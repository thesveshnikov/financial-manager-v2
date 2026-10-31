using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.User.Register;

public interface IRegisterUserAccountUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request);
}