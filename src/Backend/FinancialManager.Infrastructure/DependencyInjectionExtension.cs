using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Infrastructure.DataAccess;
using FinancialManager.Infrastructure.DataAccess.Repositories;
using FinancialManager.Infrastructure.Security.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialManager.Infrastructure;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();

            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddDbContext<FinancialManagerDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;
                
                config.UseMySQL(connectionString);
            });
        }
    }
}