using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinancialManager.Application.UseCases.Category.UpdateById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.UpdateById;

public class UpdateCategoryByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);

        var request = RequestCategoryJsonBuilder.Build();
        
        var useCase = CreateUseCase(category);

        await useCase.Execute(category.Id, request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenCategoryNotFound()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var request = RequestCategoryJsonBuilder.Build();
        
        var useCase = CreateUseCase();

        var exception = await useCase.Execute(category.Id, request).ShouldThrowAsync<NotFoundException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(string.Format(ResourceMessagesException.NOT_FOUND, "Category"));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_TitleIsEmpty()
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase(category);

        var exception = await useCase.Execute(category.Id, request).ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });
    }

    private UpdateCategoryByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Category? category = null)
    {
        var repository = new ICategoryUpdateOnlyRepositoryBuilder();
        if (category != null)
        {
            repository.GetById(category);
        }

        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new UpdateCategoryByIdUseCase(repository.Build(), unitOfWork);
    }
}