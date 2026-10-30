using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Domain.Security.Tokens;
using FinancialManager.Infrastructure.DataAccess;
using Testcontainers.MySql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class FinancialManagerApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager User1 { get; private set; } = null!;
    
    public string TOKEN_USER_NOT_FOUND_IN_DATABASE { get; private set; } = string.Empty;

    private readonly MySqlContainer _mySqlContainer;

    public FinancialManagerApplicationFactory()
    {
        _mySqlContainer = new MySqlBuilder("mysql:8.0")
            .WithDatabase("financial_manager")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _mySqlContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<FinancialManagerDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

        var (user, password) = UserBuilder.Build();
        user.Password = passwordHasher.HashPassword(password);

        var category = CategoryBuilder.Build(user);
        var expense = ExpenseBuilder.Build(user, category);
        
        expense.CategoryId  = category.Id;

        await dbContext.Users.AddAsync(user);
        await dbContext.Categories.AddAsync(category);
        await dbContext.Expenses.AddAsync(expense);

        await dbContext.SaveChangesAsync();

        var user1AccessToken = accessTokenGenerator.Generate(user);

        User1 = new UserIdentityManager(user, category, expense, password, user1AccessToken);

        TOKEN_USER_NOT_FOUND_IN_DATABASE = accessTokenGenerator.Generate(new FinancialManager.Domain.Entities.User());
    }

    Task IAsyncLifetime.DisposeAsync() => _mySqlContainer.StopAsync();
}