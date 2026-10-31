using FinancialManager.Communication.Requests;

namespace FinancialManager.Application.UseCases.Expense.UpdateById;

public interface IUpdateExpenseByIdUseCase
{
    Task Execute(Guid id, RequestExpenseJson request);
}