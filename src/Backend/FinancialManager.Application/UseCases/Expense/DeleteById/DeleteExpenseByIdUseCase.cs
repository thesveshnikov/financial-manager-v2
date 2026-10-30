using FinancialManager.Domain.Repositories.Expense;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Expense.DeleteById;

public class DeleteExpenseByIdUseCase : IDeleteExpenseByIdUseCase
{
    private readonly IExpenseWriteOnlyRepository _repository;

    public DeleteExpenseByIdUseCase(IExpenseWriteOnlyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Execute(Guid id)
    {
        var deleted = await _repository.Delete(id);
        if (deleted == false)
        {
            throw new NotFoundException(string.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        }
    }
}