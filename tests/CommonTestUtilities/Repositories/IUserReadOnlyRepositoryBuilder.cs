using FinancialManager.Domain.Entities;
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
    
    public void GetByEmail(User user)
    {
        _mock.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
    }
    
    public void GetById(User user)
    {
        _mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}