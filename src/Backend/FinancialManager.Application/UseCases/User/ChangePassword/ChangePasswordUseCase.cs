using FinancialManager.Communication.Requests;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace FinancialManager.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserUpdateOnlyRepository _repository;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordHasher passwordHasher,
        IUserUpdateOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _passwordHasher = passwordHasher;
        _repository = repository;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();
    
        Validate(request, loggedUser);

        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword);

        await _repository.UpdatePassword(loggedUser.Id, hashedPassword);
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        if (_passwordHasher.VerifyPassword(request.CurrentPassword, loggedUser.Password) == false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_CURRENT_PASSWORD));

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}