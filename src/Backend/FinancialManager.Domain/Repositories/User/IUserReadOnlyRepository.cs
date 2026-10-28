namespace FinancialManager.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    Task<bool> ExistsActiveUserWithId(Guid id);
    Task<Entities.User?> GetByEmail(string email);
    Task<Entities.User?> GetById(Guid id);
}