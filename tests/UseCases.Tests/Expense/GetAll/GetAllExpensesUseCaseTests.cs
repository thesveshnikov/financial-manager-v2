using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.Expense.GetAll;
using Shouldly;

namespace UseCases.Tests.Expense.GetAll;

public class GetAllExpensesUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var useCase = CreateUseCase([expense]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Expenses.ShouldNotBeNull();
    }
    
    private GetAllExpensesUseCase CreateUseCase(IList<FinancialManager.Domain.Entities.Expense> expenses)
    {
        (var user, _) = UserBuilder.Build();
        
        var repository = new IExpenseReadOnlyRepositoryBuilder();
        repository.GetAll(expenses);
        
        var loggedUser = ILoggedUserBuilder.Build(user);
        
        return new GetAllExpensesUseCase(repository.Build(), loggedUser);
    }
}