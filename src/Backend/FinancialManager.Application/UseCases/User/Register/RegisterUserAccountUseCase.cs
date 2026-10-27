using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        throw new NotImplementedException();
    }
}