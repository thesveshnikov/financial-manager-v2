using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Communication.Requests;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Domain.Repositories.Expense;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using FluentValidation.Results;
using Mapster;

namespace FinancialManager.Application.UseCases.Expense.UpdateById;

public class UpdateExpenseByIdUseCase : IUpdateExpenseByIdUseCase
{
    private readonly IExpenseUpdateOnlyRepository _expenseRepository;
    private readonly ICategoryReadOnlyRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateExpenseByIdUseCase(
        IExpenseUpdateOnlyRepository expenseRepository,
        ICategoryReadOnlyRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Execute(Guid id, RequestExpenseJson request)
    {
        await ValidateAndThrowOnFailures(request);
        
        var expense = await _expenseRepository.GetById(id);

        if (expense == null)
        {
            throw new NotFoundException(string.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        }
        
        request.Adapt(expense);

        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        
        var result = validator.Validate(request);

        var category = await _categoryRepository.GetById(request.CategoryId);

        if (category == null)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, string.Format(ResourceMessagesException.NOT_FOUND, "category")));
        }

        if (result.Errors.Count > 0)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    } 
}