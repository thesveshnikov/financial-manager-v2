using FinancialManager.Domain.Security.PasswordHashing;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _mock;

    public IPasswordHasherBuilder()
    {
        _mock = new Mock<IPasswordHasher>();

        _mock.Setup(passwordHasher => passwordHasher.HashPassword(It.IsAny<string>())).Returns("hashed-password");
    }

    public void VerifyPassword(string password)
    {
        _mock.Setup(repository => repository.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _mock.Object;
}