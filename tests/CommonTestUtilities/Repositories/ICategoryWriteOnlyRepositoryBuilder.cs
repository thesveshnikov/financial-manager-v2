using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Category;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ICategoryWriteOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryWriteOnlyRepository> _mock;

    public ICategoryWriteOnlyRepositoryBuilder()
    {
        _mock = new Mock<ICategoryWriteOnlyRepository>();
    }

    public ICategoryWriteOnlyRepositoryBuilder Delete(Category category)
    {
        _mock.Setup(repository => repository.Delete(category.Id)).ReturnsAsync(true);

        return this;
    }
    
    public ICategoryWriteOnlyRepository Build() => _mock.Object;
}