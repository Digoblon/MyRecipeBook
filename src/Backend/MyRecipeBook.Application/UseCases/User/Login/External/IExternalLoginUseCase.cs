namespace MyRecipeBook.Application.UseCases.User.Login.External;

public interface IExternalLoginUseCase
{
    Task<string> Execute(string name, string email);
}