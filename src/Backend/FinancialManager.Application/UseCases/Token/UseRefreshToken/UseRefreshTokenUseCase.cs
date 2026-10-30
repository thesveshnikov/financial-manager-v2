using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.RefreshToken;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.Tokens;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Token.UseRefreshToken;

public class UseRefreshTokenUseCase : IUseRefreshTokenUseCase
{
    private const int ExpirationTimeInDays = 7;

    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public UseRefreshTokenUseCase(
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
        IUserReadOnlyRepository userRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator, 
        IUnitOfWork unitOfWork)
    {
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _userRepository = userRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
    {
        var refreshToken = await _refreshTokenReadOnlyRepository.Get(request.RefreshToken);
        if (refreshToken == null)
            throw new RefreshTokenExpiredException();

        var isValid = refreshToken.CreatedAt.AddDays(ExpirationTimeInDays) >= DateTime.UtcNow;
        if (isValid == false)
            throw new RefreshTokenExpiredException();

        var user = await _userRepository.GetById(refreshToken.UserId);

        var newRefreshToken = new RefreshToken
        {
            Value = _refreshTokenGenerator.Generate(),
            UserId = refreshToken.UserId
        };

        await _refreshTokenWriteOnlyRepository.Replace(newRefreshToken);

        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(user!),
            RefreshToken = newRefreshToken.Value
        };
    }
}