namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public interface IDeleteRecipeUseCase
{
    public Task Execute(long recipeId);
}