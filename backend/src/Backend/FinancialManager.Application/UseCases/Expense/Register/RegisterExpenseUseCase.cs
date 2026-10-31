using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.Category;
using FinancialManager.Domain.Repositories.Expense;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using FluentValidation.Results;
using Mapster;

namespace FinancialManager.Application.UseCases.Expense.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _expenseRepository;
    private readonly ICategoryReadOnlyRepository _categoryRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    
    public RegisterExpenseUseCase(
        IExpenseWriteOnlyRepository expenseRepository,
        ICategoryReadOnlyRepository categoryRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request)
    {
        await ValidateAndThrowOnFailures(request);
        
        var userId = _loggedUser.GetUserId();

        var expense = request.Adapt<Domain.Entities.Expense>();
        expense.UserId = userId;
        
        await _expenseRepository.Add(expense);

        await _unitOfWork.Commit();

        return new ResponseRegisteredExpenseJson
        {
            Id = expense.Id,
            Name = expense.Name
        };
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