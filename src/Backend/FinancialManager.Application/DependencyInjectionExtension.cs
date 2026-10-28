using FinancialManager.Application.UseCases.Login.WithEmailAndPassword;
using FinancialManager.Application.UseCases.User.Profile;
using FinancialManager.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialManager.Application;

public static class DependencyInjectionExtension
{
    extension (IServiceCollection services)
    {
        public void AddApplication()
        {
            AddUseCases(services);
        }

        private void AddUseCases()
        {
            services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
        }
    }
}