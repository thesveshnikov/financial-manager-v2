using FinancialManager.Communication.Responses;
using FinancialManager.Domain.Identity;
using FinancialManager.Domain.Repositories.Category;
using Mapster;

namespace FinancialManager.Application.UseCases.Category.GetAll;

public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly ICategoryReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    
    public GetAllCategoriesUseCase(
        ICategoryReadOnlyRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    
    public async Task<ResponseCategoriesJson> Execute()
    {
        var categories = await _repository.GetAll(_loggedUser.GetUserId());

        var response = new ResponseCategoriesJson
        {
            Categories = categories.Adapt<IList<ResponseCategoryJson>>()
        };

        return response;
    }
}