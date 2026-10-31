using FinancialManager.Communication.Requests;

namespace FinancialManager.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}