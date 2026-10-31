using System.Security.Cryptography;
using FinancialManager.Domain.Security.Tokens;

namespace FinancialManager.Infrastructure.Security.Tokens.Refresh;

internal sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var token = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(token);
    }
}