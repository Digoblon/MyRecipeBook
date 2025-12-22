using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete;

public class DeleteRecipeTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";

    private readonly Guid _userIdentifier;
    private readonly string _recipeId;
    private readonly string _recipeTitle;

    public DeleteRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeId = factory.GetRecipeId();
        _recipeTitle = factory.GetRecipeTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoDelete($"{METHOD}/{_recipeId}", token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        response = await DoGet($"{METHOD}/{_recipeId}", token);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var id = IdEncrypterBuilder.Build().Encode(1000);
        
        var response = await DoDelete(method:$"{METHOD}/{id}",token:token,culture);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_NOT_FOUND",new  CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(c=> c.GetString()!.Equals(expectedMessage));
    }
}