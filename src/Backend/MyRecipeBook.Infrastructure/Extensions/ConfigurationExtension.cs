using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("DefaultConnection")!;
    }

    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}