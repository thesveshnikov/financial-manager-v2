using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Category.GetAll;

public interface IGetAllCategoriesUseCase
{
    Task<ResponseCategoriesJson> Execute();
}