using FinancialManager.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IUserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}