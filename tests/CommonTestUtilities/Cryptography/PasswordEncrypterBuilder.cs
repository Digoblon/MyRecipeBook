using MyRecipeBook.Application.Services.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    public static PasswordEncrypter Build() => new PasswordEncrypter("ABC1234");
}