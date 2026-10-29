namespace FinancialManager.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task UpdatePassword(Guid userId, string passwordHash);
}