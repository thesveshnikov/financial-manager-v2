using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Category.GetAll;
using Shouldly;

namespace UseCases.Tests.Category.GetAll;

public class GetAllCategoriesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var useCase = CreateUseCase([category]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Categories.ShouldNotBeNull();
    }
    
    private GetAllCategoriesUseCase CreateUseCase(IList<FinancialManager.Domain.Entities.Category> categories)
    {
        (var user, _) = UserBuilder.Build();
        
        var repository = new ICategoryReadOnlyRepositoryBuilder();
        repository.GetAll(categories);
        
        var loggedUser = ILoggedUserBuilder.Build(user);
        
        return new GetAllCategoriesUseCase(repository.Build(), loggedUser);
    }
}