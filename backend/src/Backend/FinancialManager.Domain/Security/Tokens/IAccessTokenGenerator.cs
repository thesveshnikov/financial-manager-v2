using FinancialManager.Domain.Entities;

namespace FinancialManager.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}