using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build() => new BCryptNet();
}