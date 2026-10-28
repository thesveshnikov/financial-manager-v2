using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPassword : ILoginWithEmailAndPassword
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _repository;

    public LoginWithEmailAndPassword(
        IPasswordHasher passwordHasher,
        IUserReadOnlyRepository repository)
    {
        _passwordHasher = passwordHasher;
        _repository = repository;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
        if (isPasswordValid == false)
            throw new InvalidLoginException();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name
        };
    }
}