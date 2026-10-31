namespace FinancialManager.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<IList<Entities.Category>> GetAll(Guid userId);
    Task<Entities.Category?> GetById(Guid id);
}