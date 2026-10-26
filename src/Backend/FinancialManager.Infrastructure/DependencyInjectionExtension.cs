using Microsoft.Extensions.DependencyInjection;

namespace FinancialManager.Infrastructure;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddInfrastructure() {}
    }
}