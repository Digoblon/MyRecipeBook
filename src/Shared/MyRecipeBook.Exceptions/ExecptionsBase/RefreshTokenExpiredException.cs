using System.Net;
using MyRecipeBook.Exceptions.ExecptionsBase;

namespace MyRecipeBook.Exceptions.ExceptionsBase;
public class RefreshTokenExpiredException : MyRecipeBookException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.INVALID_SESSION)
    {
    }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}