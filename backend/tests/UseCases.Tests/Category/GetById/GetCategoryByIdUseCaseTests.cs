using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Category.GetById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.GetById;

public class GetCategoryByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var useCase = CreateUseCase(category);

        var result = await useCase.Execute(category.Id);
        
        result.ShouldNotBeNull();
        result.Id.ShouldBe(category.Id);
        result.Name.ShouldBe(category.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenCategoryNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var useCase = CreateUseCase();
        
        var exception = await useCase.Execute(category.Id).ShouldThrowAsync<NotFoundException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(string.Format(ResourceMessagesException.NOT_FOUND, "Category"));
        });
    }
    
    private GetCategoryByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Category? category = null)
    {
        var repository = new ICategoryReadOnlyRepositoryBuilder();
        if (category != null)
        {
            repository.GetById(category);
        }
        
        return new GetCategoryByIdUseCase(repository.Build());
    }
}