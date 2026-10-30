using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Expense;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IExpenseReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpenseReadOnlyRepository> _mock;

    public IExpenseReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IExpenseReadOnlyRepository>();
    }

    public IExpenseReadOnlyRepositoryBuilder GetAll(IList<Expense> expenses)
    {
        _mock.Setup(repository => repository.GetAll(It.IsAny<Guid>())).ReturnsAsync(expenses);
        
        return this;
    }

    public IExpenseReadOnlyRepository Build() => _mock.Object;
}