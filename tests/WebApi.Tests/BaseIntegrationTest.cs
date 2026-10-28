using System.Net.Http.Json;
using FinancialManager.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests;

public abstract class BaseIntegrationTest : IClassFixture<FinancialManagerApplicationFactory>, IDisposable
{
    internal readonly FinancialManagerDbContext DbContext;

    private readonly HttpClient _httpClient;
    private readonly IServiceScope _scope;

    public BaseIntegrationTest(FinancialManagerApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<FinancialManagerDbContext>();
    }

    protected async Task<HttpResponseMessage> Post(string requestUri, object request, string culture = "en-US")
    {
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}