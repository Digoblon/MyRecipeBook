using System.Net;
using CommonTestUtilities.IdEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;

namespace WebApi.Test.Recipe.GetById;

public class GetRecipeByIdInvalidTokenTest : MyRecipeBookClassFixture
{
    private const string METHOD = "recipe";
    public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Error_Token_Invalid()
    {
        var id = IdEncrypterBuilder.Build().Encode(1);
        
        var response = await DoGet(method:$"{METHOD}/{id}",token:"tokenInvalid");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Without_Token()
    {
        var id = IdEncrypterBuilder.Build().Encode(1);
        
        var response = await DoGet(method:$"{METHOD}/{id}",token:string.Empty);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var id = IdEncrypterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        
        var response = await DoGet(method:$"{METHOD}/{id}",token:token);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}