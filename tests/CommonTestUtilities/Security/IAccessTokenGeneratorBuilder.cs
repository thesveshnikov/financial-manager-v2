using Bogus;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Security;

public class IAccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        var fakeToken = new Faker().Random.String2(32, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        mock.Setup(generator => generator.Generate(It.IsAny<User>())).Returns(fakeToken);

        return mock.Object;
    }
}