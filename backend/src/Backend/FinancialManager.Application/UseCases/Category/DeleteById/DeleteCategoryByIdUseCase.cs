using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Category.DeleteById;

public class DeleteCategoryByIdUseCase : IDeleteCategoryByIdUseCase
{
    private readonly ICategoryWriteOnlyRepository  _repository;

    public DeleteCategoryByIdUseCase(
        ICategoryWriteOnlyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Execute(Guid id)
    {
        var deleted = await _repository.Delete(id);
        if (deleted == false)
            throw new NotFoundException(String.Format(ResourceMessagesException.NOT_FOUND, "Category"));
    }
}