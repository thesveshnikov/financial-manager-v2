using FinancialManager.Communication.Requests;

namespace FinancialManager.Application.UseCases.Category.UpdateById;

public interface IUpdateCategoryByIdUseCase
{
    Task Execute(Guid id, RequestCategoryJson request);
}