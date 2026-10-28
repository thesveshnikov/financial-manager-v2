using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPassword
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}