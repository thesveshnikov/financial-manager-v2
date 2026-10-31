using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileJson> Execute();
}