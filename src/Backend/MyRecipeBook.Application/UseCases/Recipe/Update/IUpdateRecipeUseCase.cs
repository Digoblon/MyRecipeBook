using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public interface IUpdateRecipeUseCase
{
    public Task Execute(long recipeId,RequestRecipeJson request);
}