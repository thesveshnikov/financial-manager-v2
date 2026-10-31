using Bogus;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Enums;
using Type = FinancialManager.Domain.Enums.Type;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static Expense Build(User user, Category category)
    {
        return new Faker<Expense>()
            .RuleFor(expense => expense.Name, f => f.Lorem.Word())
            .RuleFor(expense => expense.Amount, f => f.Random.Decimal(1, 1000))
            .RuleFor(expense => expense.PaymentMethod, f => (PaymentMethod)f.Random.Int(0, 2))
            .RuleFor(expense => expense.Type, f => (Type)f.Random.Int(0, 1))
            .RuleFor(expense => expense.ExpenseDate, f => f.Date.Past())
            .Generate() is var expense
            ? ConfigureRelationships(expense, user, category)
            : throw new InvalidOperationException();
    }

    private static Expense ConfigureRelationships(
        Expense expense,
        User user,
        Category category)
    {
        expense.User = user;
        expense.UserId = user.Id;
        expense.Category = category;
        expense.CategoryId = category.Id;

        return expense;
    }
}