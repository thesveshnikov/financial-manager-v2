using System.Net;
using FinancialManager.Communication.Requests;
using Shouldly;

namespace WebApi.Tests.Category.Update;

public class UpdateCategoryByIdInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/categories";

    private readonly string _tokenUserNotExistDatabase;

    public UpdateCategoryByIdInvalidTokenTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var request = new RequestCategoryJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: "invalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var request = new RequestCategoryJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var request = new RequestCategoryJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}