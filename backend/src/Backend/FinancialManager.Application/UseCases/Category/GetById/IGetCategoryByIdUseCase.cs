using FinancialManager.Communication.Responses;

namespace FinancialManager.Application.UseCases.Category.GetById;

public interface IGetCategoryByIdUseCase
{
    Task<ResponseCategoryJson> Execute(Guid id);
}