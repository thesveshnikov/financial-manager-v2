using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories.Expense;
using Mapster;

namespace FinancialManager.Application.UseCases.Expense.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    
    public GetAllExpensesUseCase(
        IExpenseReadOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseExpensesJson> Execute()
    {
        var expenses = await _repository.GetAll(_loggedUser.GetUserId());

        var response = new ResponseExpensesJson
        {
            Expenses = expenses.Adapt<IList<ResponseExpenseJson>>()
        };

        return response;
    }
}