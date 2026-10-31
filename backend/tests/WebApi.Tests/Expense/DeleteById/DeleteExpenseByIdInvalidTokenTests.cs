using System.Net;
using Shouldly;

namespace WebApi.Tests.Expense.DeleteById;

public class DeleteExpenseByIdInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly string _tokenUserNotExistDatabase;

    public DeleteExpenseByIdInvalidTokenTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var response = await Delete($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: "invalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var response = await Delete($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var response = await Delete($"{REQUEST_URI}/{Guid.CreateVersion7()}", accessToken: _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}