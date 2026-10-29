using Bogus;
using FinancialManager.Communication.Enums;
using FinancialManager.Communication.Requests;
using Type = FinancialManager.Communication.Enums.Type;

namespace CommonTestUtilities.Requests;

public class RequestExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        return new Faker<RequestExpenseJson>()
            .RuleFor(request => request.Name, f => f.Lorem.Word())
            .RuleFor(request => request.Amount, f => f.Random.Decimal(0, 100))
            .RuleFor(request => request.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(request => request.Type, f => f.PickRandom<Type>())
            .RuleFor(request => request.ExpenseDate, f => f.Date.Past())
            .RuleFor(request => request.CategoryId, f => f.Random.Guid());
    }
}