using System.Reflection;
using FinancialManager.Domain.Repositories;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Domain.Security.PasswordHashing;
using FinancialManager.Infrastructure.DataAccess;
using FinancialManager.Infrastructure.DataAccess.Repositories;
using FinancialManager.Infrastructure.Security.PasswordHashing;
using FluentMigrator.Runner;
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
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<FinancialManagerDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;
                
                config.UseMySQL(connectionString);
            });

            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection")!;

                config
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.Load("FinancialManager.Infrastructure"))
                    .For.All();
            });
        }
    }
}