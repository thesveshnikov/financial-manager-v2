using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Infrastructure.Security.PasswordHashing;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialManager.Infrastructure;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddInfrastructure()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }
    }
}