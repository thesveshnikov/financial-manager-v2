using FinancialManager.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public IUserReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistsActiveUserWithEmail(string email)
    {
        _mock.Setup(repository => repository.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}