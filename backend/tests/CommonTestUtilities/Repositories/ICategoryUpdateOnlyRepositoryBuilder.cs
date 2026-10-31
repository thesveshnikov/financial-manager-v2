using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryUpdateOnlyRepository> _mock;

    public ICategoryUpdateOnlyRepositoryBuilder()
    {
        _mock = new Mock<ICategoryUpdateOnlyRepository>();
    }

    public ICategoryUpdateOnlyRepositoryBuilder GetById(Category category)
    {
        _mock.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(category);

        return this;
    }

    public ICategoryUpdateOnlyRepository Build() => _mock.Object;
}