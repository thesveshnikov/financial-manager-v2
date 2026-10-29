using FinancialManager.Communication.Requests;
using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Category.Register;

public interface IRegisterCategoryUseCase
{
    Task<ResponseCategoryJson> Execute(RequestCategoryJson request);
}