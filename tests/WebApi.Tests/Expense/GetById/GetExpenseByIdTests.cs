using System.Net;
using System.Text.Json;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Expense.GetById;

public class GetExpenseByIdTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly UserIdentityManager _user1;

    public GetExpenseByIdTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var expense = _user1.GetExpense();

        var response = await Get($"{REQUEST_URI}/{expense.Id}", accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetGuid().ShouldBe(expense.Id);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(expense.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenExpenseNotFound(string culture)
    {
        var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _user1.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = "We couldn’t find this expense."; // temporary hard code

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => string.IsNullOrWhiteSpace(error.GetString()) == false && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}