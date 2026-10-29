namespace FinancialManager.Domain.Repositories.Category;

public interface ICategoryWriteOnlyRepository
{
    Task Add(Entities.Category category);
    Task<bool> Delete(Guid id);
}