namespace FinancialManager.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task Add(Entities.User user);
    Task DeactivateAccount(Guid userId);
    Task DeleteAccount (Guid userId);
}