using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExecptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Login.DoLogin;

public class DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncrypter passwordEncrypter, IAccessTokenGenerator accessTokenGenerator)
    : IDoLoginUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encryptedPassword = passwordEncrypter.Encrypt(request.Password);
        
        var user = await repository.GetByEmailAndPassword(request.Email, encryptedPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Tokens =  new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generate(user.UserIdentifier)
            }
        };
    }
}