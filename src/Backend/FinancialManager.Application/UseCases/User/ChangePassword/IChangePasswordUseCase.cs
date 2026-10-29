using FinancialManager.Communication.Requests;

namespace FinancialManager.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}