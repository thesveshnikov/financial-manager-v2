using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;

namespace FinancialManager.Application.UseCases.Category.GetById;

public class GetCategoryByIdUseCase : IGetCategoryByIdUseCase
{
    private readonly ICategoryReadOnlyRepository _repository;

    public GetCategoryByIdUseCase(ICategoryReadOnlyRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ResponseCategoryJson> Execute(Guid id)
    {
        var category = await _repository.GetById(id);

        if (category == null)
        {
            throw new NotFoundException(string.Format(ResourceMessagesException.NOT_FOUND, "Category"));
        }
        
        return new ResponseCategoryJson
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}