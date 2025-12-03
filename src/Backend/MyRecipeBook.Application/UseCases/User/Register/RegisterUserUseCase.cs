using AutoMapper;
using FluentValidation.Results;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExecptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository  _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncrypter _passwordEncrypter;

    public RegisterUserUseCase(IUserReadOnlyRepository readOnlyRepository, 
        IUserWriteOnlyRepository  writeOnlyRepository, 
        PasswordEncrypter passwordEncrypter,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        
        //validar a request
        await Validate(request);
        
        
        //mapear a request em uma entidade
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncrypter.Encrypt(request.Password);
        
        //salvar no banco de dados
        await _writeOnlyRepository.Add(user);
        await _unitOfWork.Commit();
        
        
        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();

        var result = validator.Validate(request);
        
        var emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty,ResourceMessagesExeption.EMAIL_ALREADY_REGISTERED));
        

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
        
    }
}