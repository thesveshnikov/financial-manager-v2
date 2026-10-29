using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryReadOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryReadOnlyRepository> _mock;

    public ICategoryReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<ICategoryReadOnlyRepository>();
    }

    public ICategoryReadOnlyRepositoryBuilder GetAll(IList<Category> categories)
    {
        _mock.Setup(repository => repository.GetAll(It.IsAny<Guid>())).ReturnsAsync(categories);
        
        return this;
    }

    public ICategoryReadOnlyRepository Build() => _mock.Object;
}