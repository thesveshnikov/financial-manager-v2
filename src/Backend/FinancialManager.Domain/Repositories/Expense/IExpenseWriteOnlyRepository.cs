namespace FinancialManager.Domain.Repositories.Expense;

public interface IExpenseWriteOnlyRepository
{
    Task Add(Entities.Expense expense);
}