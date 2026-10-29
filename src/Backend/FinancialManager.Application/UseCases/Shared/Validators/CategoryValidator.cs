using FinancialManager.Communication.Requests;
using FinancialManager.Exception;
using FluentValidation;

namespace FinancialManager.Application.UseCases.Shared.Validators;

public class CategoryValidator : AbstractValidator<RequestCategoryJson>
{
    public CategoryValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED)
            .MaximumLength(255)
            .WithMessage(ResourceMessagesException.VALIDATION_NAME_MAX_LENGTH);
    }
}