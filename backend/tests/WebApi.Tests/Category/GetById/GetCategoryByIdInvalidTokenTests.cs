using System.Net;
using Shouldly;

namespace WebApi.Tests.Category.GetById;

public class GetCategoryByIdInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/categories";

    private readonly string _tokenUserNotExistDatabase;

    public GetCategoryByIdInvalidTokenTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: "invalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var response = await Get($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}