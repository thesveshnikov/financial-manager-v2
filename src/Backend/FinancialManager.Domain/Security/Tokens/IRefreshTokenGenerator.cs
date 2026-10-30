namespace FinancialManager.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate();
}