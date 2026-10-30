using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinancialManager.Exception;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Expense.Register;

public class RegisterExpenseTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly UserIdentityManager _user1;

    public RegisterExpenseTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.CategoryId = _user1.GetCategory().Id;

        var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);

        var expenseId = responseData.RootElement.GetProperty("id").GetGuid();

        var expenseExists = await DbContext.Expenses.AnyAsync(expense =>
            expense.Id == expenseId &&
            expense.Active &&
            expense.Name.Equals(request.Name) &&
            expense.UserId == _user1.GetId());

        expenseExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.CategoryId = _user1.GetCategory().Id;
        
        request.Name = string.Empty;

        var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => string.IsNullOrWhiteSpace(error.GetString()) == false && error.GetString()!.Equals(expectedErrorMessage));
        });

        var expenseExists = await DbContext.Expenses.AnyAsync(expense =>
            expense.Active &&
            expense.UserId == _user1.GetId() &&
            expense.Name.Equals(request.Name));

        expenseExists.ShouldBeFalse();
    }
}