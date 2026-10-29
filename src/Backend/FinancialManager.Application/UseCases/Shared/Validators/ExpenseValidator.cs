using FinancialManager.Communication.Requests;
using FinancialManager.Exception;
using FluentValidation;

namespace FinancialManager.Application.UseCases.Shared.Validators;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    public ExpenseValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        RuleFor(request => request.Amount).GreaterThan(0).WithMessage(ResourceMessagesException.VALIDATION_AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(request => request.PaymentMethod).IsInEnum().WithMessage(string.Format(ResourceMessagesException.VALIDATION_ENUM_INVALID, "payment method"));
        RuleFor(request => request.Type).IsInEnum().WithMessage(string.Format(ResourceMessagesException.VALIDATION_ENUM_INVALID, "type"));
        RuleFor(request => request.ExpenseDate).LessThan(DateTime.Now).WithMessage(ResourceMessagesException.VALIDATION_EXPENSE_DATE_INVALID);
        RuleFor(request => request.CategoryId).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_CATEGORY_ID_REQUIRED);
    }
}