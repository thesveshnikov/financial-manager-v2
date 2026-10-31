namespace FinancialManager.Domain.Repositories.Expense;

public interface IExpenseReadOnlyRepository
{
    Task<IList<Entities.Expense>> GetAll(Guid userId);
    Task<Entities.Expense?> GetById(Guid id);
}