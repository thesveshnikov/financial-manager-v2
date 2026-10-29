using FinancialManager.Application.UseCases.Shared.Validators;
using FinancialManager.Communication.Requests;
using FluentValidation;

namespace FinancialManager.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}