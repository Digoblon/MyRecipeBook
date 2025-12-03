namespace MyRecipeBook.Exceptions.ExecptionsBase;

public class ErrorOnValidationException : MyRecipeBookException
{
    public IList<string> ErrorMessages { get; set; }
    
    public ErrorOnValidationException(IList<string> errors)
    {
        ErrorMessages = errors;
    }
}