using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Expense.GetAll;

public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}