using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Category.DeleteById;

public class DeleteCategoryByIdTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/categories";

    private readonly UserIdentityManager _user1;

    public DeleteCategoryByIdTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var category = _user1.GetCategory();

        var response = await Delete($"{REQUEST_URI}/{category.Id}", accessToken: _user1.GetAccessToken()); 

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var categoryExists = await DbContext.Categories.AnyAsync(entity => entity.Id == category.Id);

        categoryExists.ShouldBeFalse();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenCategoryNotFound(string culture)
    {
        var response = await Delete($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _user1.GetAccessToken(), culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = "We couldn’t find this Category."; // temporary hard code

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => string.IsNullOrWhiteSpace(error.GetString()) == false && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}