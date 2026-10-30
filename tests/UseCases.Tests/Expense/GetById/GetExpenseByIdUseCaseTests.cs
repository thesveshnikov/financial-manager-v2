using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Expense.GetById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Expense.GetById;

public class GetExpenseByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var useCase = CreateUseCase(expense);

        var result = await useCase.Execute(expense.Id);
        
        result.ShouldNotBeNull();
        result.Id.ShouldBe(expense.Id);
        result.Name.ShouldBe(expense.Name);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenExpenseNotFound()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var useCase = CreateUseCase();
        
        var exception = await useCase.Execute(expense.Id).ShouldThrowAsync<NotFoundException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(string.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        });
    }
    
    private GetExpenseByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Expense? expense = null)
    {
        var repository = new IExpenseReadOnlyRepositoryBuilder();
        if (expense != null)
        {
            repository.GetById(expense);
        }
        
        return new GetExpenseByIdUseCase(repository.Build());
    }
}