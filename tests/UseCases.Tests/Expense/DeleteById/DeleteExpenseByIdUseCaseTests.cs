using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Expense.DeleteById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Expense.DeleteById;

public class DeleteExpenseByIdUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var useCase = CreateUseCase(expense);

        await useCase.Execute(expense.Id).ShouldNotThrowAsync();
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
            errorMessages.ShouldContain(String.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        });
    }
    
    private DeleteExpenseByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Expense? expense = null)
    {
        var repository = new IExpenseWriteOnlyRepositoryBuilder();
        if (expense != null)
        {
                repository.Delete(expense);
        }
        
        return new DeleteExpenseByIdUseCase(repository.Build());
    }
}