using FinancialManager.Domain.Entities;

namespace FinancialManager.Domain.Identity;

public interface ILoggedUser
{
    Task<User> Get();
    Guid GetUserId();
}