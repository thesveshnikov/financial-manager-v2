using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Category.DeleteById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.DeleteById;

public class IDeleteCategoryByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var useCase = CreateUseCase(category);

        await useCase.Execute(category.Id).ShouldNotThrowAsync();
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
            errorMessages.ShouldContain(String.Format(ResourceMessagesException.NOT_FOUND, "Category"));
        });
    }
    
    private DeleteCategoryByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Category? category = null)
    {
        var repository = new ICategoryWriteOnlyRepositoryBuilder();
        if (category != null)
        {
            repository.Delete(category);
        }
        
        return new DeleteCategoryByIdUseCase(repository.Build());
    }
}