using System.Net;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.DeleteAccount;

public class DeleteUserAccountTests : BaseIntegrationTest
{
    private const string REQUEST_URI = "api/users";

    private readonly UserIdentityManager _user1;

    public DeleteUserAccountTests(FinancialManagerApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var response = await Delete(REQUEST_URI, _user1.GetAccessToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await WaitUntilAccountIsDeleted(_user1.GetId());

        var categoriesExist = await DbContext.Categories.AnyAsync(recipe => recipe.UserId == _user1.GetId());
        categoriesExist.ShouldBeFalse();

        var expensesExist = await DbContext.Expenses.AnyAsync(expense => expense.UserId == _user1.GetId());
        expensesExist.ShouldBeFalse();
    }

    private async Task WaitUntilAccountIsDeleted(Guid userId)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            var accountExists = await DbContext.Users.AnyAsync(user => user.Id == userId);
            if (accountExists == false)
                return;

            await Task.Delay(500);
        }

        throw new TimeoutException("The account was not removed in time.");
    }
}