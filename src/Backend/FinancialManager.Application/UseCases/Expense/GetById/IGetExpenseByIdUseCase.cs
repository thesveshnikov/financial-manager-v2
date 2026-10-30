using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Expense.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpenseJson> Execute(Guid id);
}