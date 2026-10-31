using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FinancialManager.Communication.Requests;
using FinancialManager.Exception;
using Shouldly;
using UseCases.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/authentication";

    private readonly HttpClient _httpClient;
    private readonly UserIdentityManager _user1;

    public LoginWithEmailAndPasswordTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _httpClient = factory.CreateClient();
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _user1.GetEmail(),
            Password = _user1.GetPassword()
        };

        var response = await Post(REQUEST_URI, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.GetName());
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenUserDontExist(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);

        var response = await Post(REQUEST_URI, request, culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedErrorMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_LOGIN_INVALID", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => string.IsNullOrWhiteSpace(error.GetString()) == false && error.GetString()!.Equals(expectedErrorMessage));
        });
    }
}