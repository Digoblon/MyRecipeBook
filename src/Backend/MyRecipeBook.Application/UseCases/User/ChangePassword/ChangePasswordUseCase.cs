using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;
using MyRecipeBook.Exceptions.ExecptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;
        
    public ChangePasswordUseCase(ILoggedUser loggedUser,IUserUpdateOnlyRepository repository, IUnitOfWork unitOfWork,IPasswordEncrypter passwordEncrypter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.User();
        
        Validate(request, loggedUser);
        
        var user = await _repository.GetById(loggedUser.Id);
        
        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);
        
        _repository.Update(user);
        
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);
        
        var currentPasswordEncrypted = _passwordEncrypter.Encrypt(request.Password);
        
        if(!currentPasswordEncrypted.Equals(loggedUser.Password))
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty,ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}