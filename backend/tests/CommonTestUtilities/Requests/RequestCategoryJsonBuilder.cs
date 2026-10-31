using Bogus;
using FinancialManager.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestCategoryJsonBuilder
{
    public static RequestCategoryJson Build()
    {
        return new Faker<RequestCategoryJson>()
            .RuleFor(c => c.Name, f => f.Lorem.Word());
    }
}