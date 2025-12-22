using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;
    public RecipeRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

    public async Task Delete(long recipeId)
    {
        var recipe = await _dbContext.Recipes.FindAsync(recipeId);

        _dbContext.Recipes.Remove(recipe!);
    }
    

    public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filter)
    {
        var query = _dbContext
            .Recipes
            .AsNoTracking()
            .Include(r=> r.Ingredients)
            .Where(r =>r.Active && r.UserId == user.Id);

        if (filter.Difficulties.Any())
        {
            query = query.Where(r => r.Difficulty.HasValue && filter.Difficulties.Contains(r.Difficulty.Value));
        }
        
        if (filter.CookingTimes.Any())
        {
            query = query.Where(r => r.CookingTime.HasValue && filter.CookingTimes.Contains(r.CookingTime.Value));
        }

        if (filter.DishTypes.Any())
        {
            query = query.Where(r=>r.DishTypes.Any(dishType => filter.DishTypes.Contains(dishType.Type)));
        }

        if (filter.RecipeTitle_Ingredient.NotEmpty())
        {
            query = query.Where(r=> r.Title.Contains(filter.RecipeTitle_Ingredient)
                                    ||r.Ingredients.Any(ingredient => ingredient.Item.Contains(filter.RecipeTitle_Ingredient)));
        }
        
        return await query.ToListAsync();
    }

    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>r.Active && r.Id == recipeId && r.Id == user.Id );
    }
    
    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(r =>r.Active && r.Id == recipeId && r.Id == user.Id );
    }
    
    public void Update(Recipe recipe) => _dbContext.Recipes.Update(recipe);
    
    public async Task<IList<Recipe>> GetForDashboard(User user)
    {
        return await _dbContext
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id)
            .OrderByDescending(r => r.CreatedOn)
            .Take(5)
            .ToListAsync();
    }


    private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
    {
        return _dbContext
            .Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.DishTypes);
    }
}