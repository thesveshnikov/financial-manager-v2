using CommonTestUtilities.Repositories;
using FinancialManager.Application.UseCases.User.DeleteAccount;
using Shouldly;

namespace UseCases.Tests.User.DeleteAccount;

public class DeleteUserAccountPermanentlyUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();

        await useCase.Execute(Guid.NewGuid()).ShouldNotThrowAsync();
    }

    private static DeleteUserAccountPermanentlyUseCase CreateUseCase()
    {
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new DeleteUserAccountPermanentlyUseCase(userWriteOnlyRepository, unitOfWork);
    }
}