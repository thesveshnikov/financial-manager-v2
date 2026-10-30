using System.Net;
using System.Text.Json;
using Shouldly;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Expense.GetAll;

public class GetAllExpensesTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly UserIdentityManager _user1;

    public GetAllExpensesTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var expense = _user1.GetExpense();

        var response = await Get($"{REQUEST_URI}", accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("expenses")[0].GetProperty("id").GetGuid().ShouldBe(expense.Id);
        responseData.RootElement.GetProperty("expenses")[0].GetProperty("name").GetString().ShouldBe(expense.Name);
    }
}