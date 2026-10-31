using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Communication.Requests;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Mapster;

namespace FinancialManager.Application.UseCases.Category.UpdateById;

public class UpdateCategoryByIdUseCase : IUpdateCategoryByIdUseCase
{   
    private readonly ICategoryUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryByIdUseCase(
        ICategoryUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(Guid id, RequestCategoryJson request)
    {
        ValidateAndThrowOnFailures(request);

        var category = await _repository.GetById(id);

        if (category == null)
        {
            throw new NotFoundException(string.Format(ResourceMessagesException.NOT_FOUND, "Category"));
        }

        request.Adapt(category);

        await _unitOfWork.Commit();
    }

    private static void ValidateAndThrowOnFailures(RequestCategoryJson request)
    {
        var result = new CategoryValidator().Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
        }
    }
}