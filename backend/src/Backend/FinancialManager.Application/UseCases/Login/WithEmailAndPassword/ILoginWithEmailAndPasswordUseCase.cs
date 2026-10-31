using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}