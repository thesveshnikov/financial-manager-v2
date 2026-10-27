using FinancialManager.Communication.Requests;
using FinancialManager.Exception;
using FluentValidation;

namespace FinancialManager.Application.UseCases.User.Register;

public class RegisterUserAccountValidator : AbstractValidator<RequestRegisterUserAccountJson>
{
    public RegisterUserAccountValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED);
        RuleFor(user => user.Password).NotEmpty().WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
        RuleFor(user => user.DateOfBirth).Must(For13Years).WithMessage(ResourceMessagesException.VALIDATION_DATE_OF_BIRTH_INVALID);
        When(user => string.IsNullOrWhiteSpace(user.Email) == false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.VALIDATION_EMAIL_INVALID);
        });
    }
    
    private bool For13Years(DateTime date)
    {
        var limit = DateTime.Now.AddYears(-13);
        return date <= limit;
    }
}