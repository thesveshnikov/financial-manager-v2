using FinancialManager.Application.UseCases.Login.WithEmailAndPassword;
using FinancialManager.Application.UseCases.User.ChangePassword;
using FinancialManager.Application.UseCases.User.Profile;
using FinancialManager.Application.UseCases.User.Register;
using FinancialManager.Application.UseCases.User.Update;
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
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
        }
    }
}