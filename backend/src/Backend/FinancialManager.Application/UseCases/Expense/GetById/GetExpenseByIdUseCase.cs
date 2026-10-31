using FinancialManager.Communication.Enums;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Repositories.Expense;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Type = FinancialManager.Communication.Enums.Type;

namespace FinancialManager.Application.UseCases.Expense.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;

    public GetExpenseByIdUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResponseExpenseJson> Execute(Guid id)
    {
        var expense = await _repository.GetById(id);

        if (expense == null)
        {
            throw new NotFoundException(string.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        }

        return new ResponseExpenseJson
        {
            Id = expense.Id,
            Name = expense.Name,
            Amount = expense.Amount,
            PaymentMethod = (PaymentMethod)expense.PaymentMethod,
            Type = (Type)expense.Type,
            ExpenseDate = expense.ExpenseDate
        };
    }
}