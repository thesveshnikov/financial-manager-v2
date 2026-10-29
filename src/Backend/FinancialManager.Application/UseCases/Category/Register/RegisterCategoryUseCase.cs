using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Exception.ExceptionsBase;
using Mapster;

namespace FinancialManager.Application.UseCases.Category.Register;

public class RegisterCategoryUseCase : IRegisterCategoryUseCase
{
    private readonly ICategoryWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public RegisterCategoryUseCase(
        ICategoryWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseCategoryJson> Execute(RequestCategoryJson request)
    {
        ValidateAndThrowOnFailures(request);

        var category = request.Adapt<Domain.Entities.Category>();
        category.UserId = _loggedUser.GetUserId();

        await _repository.Add(category);

        await _unitOfWork.Commit();

        return new ResponseCategoryJson
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    private static void ValidateAndThrowOnFailures(RequestCategoryJson request)
    {
        var result = new CategoryValidator().Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}