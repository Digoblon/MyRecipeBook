using Bogus;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Enums;

namespace CommonTestUtilities.Requests;

public class RequestFilterRecipeJsonBuilder
{
    public static RequestFilterRecipeJson Build()
    {
        return new Faker<RequestFilterRecipeJson>()
            .RuleFor(r => r.CookingTimes, f => f.Make(1, () => f.PickRandom<CookingTime>()))
            .RuleFor(r => r.Difficulties, f => f.Make(1, () => f.PickRandom<Difficulty>()))
            .RuleFor(r => r.DishTypes, f => f.Make(1, () => f.PickRandom<DishType>()))
            .RuleFor(r => r.RecipeTitile_Ingredient, f => f.Lorem.Word());

    }

}