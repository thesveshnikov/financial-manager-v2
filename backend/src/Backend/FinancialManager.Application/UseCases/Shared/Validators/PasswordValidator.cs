using FinancialManager.Exception;
using FluentValidation;

namespace FinancialManager.Application.UseCases.Shared.Validators;

public static class PasswordValidator
{
    extension<TRequest>(IRuleBuilderInitial<TRequest, string> ruleBuilder)
    {
        internal IRuleBuilderOptions<TRequest, string> Password()
        {
            return ruleBuilder
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)
                .MinimumLength(8)
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH)
                .Matches("[A-Z]")
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MUST_CONTAIN_ONE_UPPERCASE_LETTER)
                .Matches("[a-z]")
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MUST_CONTAIN_ONE_LOWERCASE_LETTER)
                .Matches("[0-9]")
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MUST_CONTAIN_ONE_NUMBER)
                .Matches("[^a-zA-Z0-9]")
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_MUST_CONTAIN_ONE_SPECIAL_CHARACTER);
        }
    }
}