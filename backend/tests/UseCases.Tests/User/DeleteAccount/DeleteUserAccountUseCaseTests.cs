using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.User.DeleteAccount;
using Shouldly;

namespace UseCases.Tests.User.DeleteAccount;

public class DeleteUserAccountUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();

        await useCase.Execute().ShouldNotThrowAsync();
    }

    private static DeleteUserAccountUseCase CreateUseCase()
    {
        var (user, _) = UserBuilder.Build();

        var loggedUser = ILoggedUserBuilder.Build(user);
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();

        return new DeleteUserAccountUseCase(loggedUser, userWriteOnlyRepository);
    }
}