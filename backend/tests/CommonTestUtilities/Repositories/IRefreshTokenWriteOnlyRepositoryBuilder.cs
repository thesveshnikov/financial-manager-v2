using FinancialManager.Domain.Repositories.RefreshToken;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IRefreshTokenWriteOnlyRepositoryBuilder
{
    public static IRefreshTokenWriteOnlyRepository Build()
    {
        var mock = new Mock<IRefreshTokenWriteOnlyRepository>();

        return mock.Object;
    }
}