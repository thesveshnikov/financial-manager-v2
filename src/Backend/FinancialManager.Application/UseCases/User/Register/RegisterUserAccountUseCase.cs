using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Exception.ExceptionsBase;
using Mapster;

namespace FinancialManager.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterUserAccountUseCase(
        IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _repository.Add(user);
        
        await _unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }

    private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}