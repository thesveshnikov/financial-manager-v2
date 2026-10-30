using System.Net;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinancialManager.Application.UseCases.Expense.UpdateById;
using FinancialManager.Exception;
using FinancialManager.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.Expense.Update;

public class UpdateExpenseUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);

        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(expense);

        await useCase.Execute(expense.Id, request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenExpenseNotFound()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase();

        var exception = await useCase.Execute(expense.Id, request).ShouldThrowAsync<NotFoundException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.NotFound);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(string.Format(ResourceMessagesException.NOT_FOUND, "expense"));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_TitleIsEmpty()
    {
        (var user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        var request = RequestExpenseJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase(expense);

        var exception = await useCase.Execute(expense.Id, request).ShouldThrowAsync<ErrorOnValidationException>();
        
        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);
        
        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
        });
    }

    private UpdateExpenseByIdUseCase CreateUseCase(FinancialManager.Domain.Entities.Expense? expense = null)
    {
        var (user, _) = UserBuilder.Build();
        var category = CategoryBuilder.Build(user);
        
        var expenseRepository = new IExpenseUpdateOnlyRepositoryBuilder();
        if (expense != null)
        {
            expenseRepository.GetById(expense);
        }
        
        var categoryRepository = new ICategoryReadOnlyRepositoryBuilder();
        categoryRepository.GetById(category);
        
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new UpdateExpenseByIdUseCase(expenseRepository.Build(), categoryRepository.Build(), unitOfWork);
    }
}