using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using MyRecipeBook.API.BackgroundServices;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;
using Scalar.AspNetCore;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.OperationFilter<IdsFilter>();
    
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = AUTHENTICATION_TYPE,
        Name = "Authorization",
        Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer'  [space] and then your token in the text input below.
                        Example: 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = AUTHENTICATION_TYPE
            },
            Scheme = "oauth2",
            Name = AUTHENTICATION_TYPE,
            In = ParameterLocation.Header
        },
        Array.Empty<string>()
    }});
});

builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddMvc(options =>options.Filters.Add<ExceptionFilter>());


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

if (!builder.Configuration.IsUnitTestEnvironment())
{
    builder.Services.AddHostedService<DeleteUserService>();

    AddGoogleAuthentication();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.MapSwagger("/openapi/{documentName}.json");
    app.MapScalarApiReference();
    
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if(builder.Configuration.IsUnitTestEnvironment())
        return;
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    DatabaseMigration.Migrate(builder.Configuration.ConnectionString(),serviceScope.ServiceProvider);
}

void AddGoogleAuthentication()
{
    var clientId = builder.Configuration.GetValue<string>("Settings:Google:ClientId")!;
    var clientSecret = builder.Configuration.GetValue<string>("Settings:Google:ClientSecret")!;

    builder.Services.AddAuthentication(config =>
        {
            config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = clientId;
            googleOptions.ClientSecret = clientSecret;
        });

}

public partial class Program
{
    protected Program() { }
}