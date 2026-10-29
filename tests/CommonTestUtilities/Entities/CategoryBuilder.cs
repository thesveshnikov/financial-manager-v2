using Bogus;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Enums;
using Type = FinancialManager.Domain.Enums.Type;

namespace CommonTestUtilities.Entities;

public class CategoryBuilder
{
    public static Category Build(User user)
    {
        var category = new Faker<Category>()
            .RuleFor(category => category.Name, f => f.Lorem.Word())
            .RuleFor(category => category.UserId, _ => user.Id)
            .Generate();

        category.User = user;

        category.Expenses = new Faker<Expense>()
            .RuleFor(expense => expense.Name, f => f.Lorem.Word())
            .RuleFor(expense => expense.Amount, f => f.Random.Decimal(1, 1000))
            .RuleFor(expense => expense.PaymentMethod, f => (PaymentMethod)f.Random.Int(0, 2))
            .RuleFor(expense => expense.Type, f => (Type)f.Random.Int(0, 1))
            .RuleFor(expense => expense.ExpenseDate, f => f.Date.Past())
            .RuleFor(expense => expense.UserId, _ => user.Id)
            .Generate(2);

        foreach (var expense in category.Expenses)
        {
            expense.User = user;
            expense.Category = category;
            expense.CategoryId = category.Id;
        }

        return category;
    }
}