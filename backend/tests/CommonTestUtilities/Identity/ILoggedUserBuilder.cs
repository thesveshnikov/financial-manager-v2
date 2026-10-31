using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Identity;
using Moq;

namespace CommonTestUtilities.Identity;

public class ILoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);

        mock.Setup(loggedUser => loggedUser.GetUserId()).Returns(user.Id);

        return mock.Object;
    }
}