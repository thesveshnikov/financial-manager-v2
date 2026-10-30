namespace FinancialManager.Application.UseCases.Expense.Delete;

public interface IDeleteExpenseByIdUseCase
{
    Task Execute(Guid id);
}