namespace FinancialManager.Application.UseCases.Expense.DeleteById;

public interface IDeleteExpenseByIdUseCase
{
    Task Execute(Guid id);
}