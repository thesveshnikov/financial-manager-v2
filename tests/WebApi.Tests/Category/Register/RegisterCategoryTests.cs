using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinancialManager.Exception;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Category.Register;

public class RegisterCategoryTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/categories";

    private readonly UserIdentityManager _user1;

    public RegisterCategoryTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestCategoryJsonBuilder.Build();

        var response = await Post(REQUEST_URI, request, accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);

        var categoryId = responseData.RootElement.GetProperty("id").GetGuid();

        var categoryExists = await DbContext.Categories.AnyAsync(category =>
            category.Id == categoryId &&
            category.Active &&
            category.Name.Equals(request.Name) &&
            category.UserId == _user1.GetId());

        categoryExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        var request = RequestCategoryJsonBuilder.Build();
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

        var categoryExists = await DbContext.Categories.AnyAsync(category =>
            category.Active &&
            category.UserId == _user1.GetId() &&
            category.Name.Equals(request.Name));

        categoryExists.ShouldBeFalse();
    }
}