
using System.Net;
using FinancialManager.Communication.Requests;
using Shouldly;

namespace WebApi.Tests.Expense.Update;

public class UpdateExpenseInvalidTokenTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/expenses";

    private readonly string _tokenUserNotExistDatabase;

    public UpdateExpenseInvalidTokenTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsInvalid()
    {
        var request = new RequestExpenseJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: "invalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenAccessTokenIsMissing()
    {
        var request = new RequestExpenseJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, accessToken: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Validate_ShouldBeAnErrorResponse_WhenUserFromAccessTokenDoesNotExist()
    {
        var request = new RequestExpenseJson();

        var response = await Put($"{REQUEST_URI}/{Guid.CreateVersion7()}", request, _tokenUserNotExistDatabase);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}