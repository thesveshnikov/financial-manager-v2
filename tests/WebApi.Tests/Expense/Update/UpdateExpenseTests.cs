using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinancialManager.Exception;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Expense.Update;

public class UpdateExpenseTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly UserIdentityManager _user1;

    public UpdateExpenseTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var recipe = _user1.GetExpense();
        var request = RequestExpenseJsonBuilder.Build();
        request.CategoryId = _user1.GetCategory().Id;

        var response = await Put($"{REQUEST_URI}/{recipe.Id}", request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var recipeUpdated = await DbContext.Expenses.AnyAsync(entity =>
            entity.Id == recipe.Id &&
            entity.Active &&
            entity.Name.Equals(request.Name) &&
            entity.UserId == _user1.GetId());

        recipeUpdated.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenExpenseNotFound(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.CategoryId = _user1.GetCategory().Id;

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: _user1.GetAccessToken(), culture: culture);

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

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var recipe = _user1.GetExpense();

        var request = RequestExpenseJsonBuilder.Build();
        request.CategoryId = _user1.GetCategory().Id;
        request.Name = string.Empty;

        var response = await Put($"{REQUEST_URI}/{recipe.Id}", request, accessToken: _user1.GetAccessToken(), culture: culture);

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
    }
}