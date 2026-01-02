using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    public Task<ResponseRecipeJson> Execute(long recipeId);
}