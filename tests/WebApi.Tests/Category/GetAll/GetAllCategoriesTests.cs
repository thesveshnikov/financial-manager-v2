using System.Net;
using System.Text.Json;
using Shouldly;
using WebApi.Tests.Resources;

namespace WebApi.Tests.Category.GetAll;

public class GetAllCategoriesTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/categories";

    private readonly UserIdentityManager _user1;

    public GetAllCategoriesTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var category = _user1.GetCategory();

        var response = await Get($"{REQUEST_URI}", accessToken: _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("categories")[0].GetProperty("id").GetGuid().ShouldBe(category.Id);
        responseData.RootElement.GetProperty("categories")[0].GetProperty("name").GetString().ShouldBe(category.Name);
    }
}