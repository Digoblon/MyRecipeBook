namespace MyRecipeBook.Domain.Dtos;

public record GeneratedInstructionsDto
{
    public int Step { get; init; }
    public string Text { get; init; } = string.Empty;
}