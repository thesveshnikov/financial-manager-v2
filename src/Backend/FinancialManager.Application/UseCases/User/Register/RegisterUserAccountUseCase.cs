using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Domain.Security.Tokens;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using FluentValidation.Results;
using Mapster;

namespace FinancialManager.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterUserAccountUseCase(
        IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>();

        user.Password = _passwordHasher.HashPassword(request.Password);

        await _writeOnlyRepository.Add(user);
        
        await _unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user)
            }
        };
    }

    private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        var emailExists = await _readOnlyRepository.ExistsActiveUserWithEmail(request.Email);
        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
        }
        
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}