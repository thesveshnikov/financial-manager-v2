namespace FinancialManager.Domain.Repositories.Category;

public interface ICategoryUpdateOnlyRepository
{
    Task<Entities.Category?> GetById(Guid id);
}