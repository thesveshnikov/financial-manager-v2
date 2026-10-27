using FinancialManager.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialManager.Application;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddApplication()
        {
            services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
        }
    }
}