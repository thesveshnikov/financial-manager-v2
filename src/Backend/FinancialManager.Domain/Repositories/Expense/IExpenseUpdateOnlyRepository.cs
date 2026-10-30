namespace FinancialManager.Domain.Repositories.Expense;

public interface IExpenseUpdateOnlyRepository
{
    Task<Entities.Expense?> GetById(Guid id);
}