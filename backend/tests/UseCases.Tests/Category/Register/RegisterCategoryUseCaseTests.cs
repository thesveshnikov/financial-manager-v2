using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinancialManager.Application.UseCases.Category.Register;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Category.Register;

public class RegisterCategoryUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCategoryJsonBuilder.Build();
        
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(result.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var request = RequestCategoryJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();
        
        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });
    }
    
    private RegisterCategoryUseCase CreateUseCase()
    {
        (var user, _) = UserBuilder.Build();
        
        var repository = new ICategoryWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);
        
        return new RegisterCategoryUseCase(repository, unitOfWork, loggedUser);
    }
}