using AutoMapper;
using CommonTestUtilities.IdEncryption;
using MyRecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncrypter = IdEncrypterBuilder.Build();
        
        return new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(idEncrypter));
        }).CreateMapper();
    }
}