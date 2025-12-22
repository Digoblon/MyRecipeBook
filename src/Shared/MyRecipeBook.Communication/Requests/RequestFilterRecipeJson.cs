using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Requests;

public class RequestFilterRecipeJson
{
    public string? RecipeTitile_Ingredient { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}