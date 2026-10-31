using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.RefreshToken;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Domain.Security.Tokens;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    
    public LoginWithEmailAndPasswordUseCase(
        IPasswordHasher passwordHasher,
        IUserReadOnlyRepository userRepository,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _userRepository.GetByEmail(request.Email);
        if (user == null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
        if (isPasswordValid == false)
            throw new InvalidLoginException();

        var refreshToken = await GenerateAndSaveRefreshToken(user.Id);

        await _unitOfWork.Commit();
        
        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user),
                RefreshToken = refreshToken
            }
        };
    }

    private async Task<string> GenerateAndSaveRefreshToken(Guid userId)
    {
        var refreshToken = new RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = userId
        };

        await _refreshTokenRepository.Replace(refreshToken);

        return refreshToken.Value;
    }
}