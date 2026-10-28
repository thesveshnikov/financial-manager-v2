using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Security.Tokens;
using FinancialManager.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinancialManager.Infrastructure.Identity;

internal sealed class LoggedUser : ILoggedUser
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly FinancialManagerDbContext _dbContext;

    public LoggedUser(IAccessTokenProvider accessTokenProvider, FinancialManagerDbContext dbContext)
    {
        _accessTokenProvider = accessTokenProvider;
        _dbContext = dbContext;
    }

    public async Task<User> Get()
    {
        var userId = GetUserId();

        return await _dbContext.Users.FirstAsync(user => user.Active && user.Id == userId);
    }

    public Guid GetUserId()
    {
        var accessToken = _accessTokenProvider.GetToken();

        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(accessToken);

        var subject = jsonWebToken.Claims.First(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub));

        return Guid.Parse(subject.Value);
    }
}