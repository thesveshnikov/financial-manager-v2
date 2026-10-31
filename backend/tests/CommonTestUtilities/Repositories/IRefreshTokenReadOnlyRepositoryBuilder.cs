using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.RefreshToken;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IRefreshTokenReadOnlyRepositoryBuilder
{
    private readonly Mock<IRefreshTokenReadOnlyRepository> _mock;

    public IRefreshTokenReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IRefreshTokenReadOnlyRepository>();
    }

    public IRefreshTokenReadOnlyRepositoryBuilder Get(RefreshToken refreshToken)
    {
        _mock.Setup(repository => repository.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public IRefreshTokenReadOnlyRepository Build() => _mock.Object;
}