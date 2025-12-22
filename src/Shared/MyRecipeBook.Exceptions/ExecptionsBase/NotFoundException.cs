using System.Net;

namespace MyRecipeBook.Exceptions.ExecptionsBase;

public class NotFoundException : MyRecipeBookException
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}