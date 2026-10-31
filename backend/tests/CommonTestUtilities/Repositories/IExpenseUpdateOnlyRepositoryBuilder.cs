using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Expense;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IExpenseUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpenseUpdateOnlyRepository> _mock;

    public IExpenseUpdateOnlyRepositoryBuilder()
    {
        _mock = new Mock<IExpenseUpdateOnlyRepository>();
    }

    public IExpenseUpdateOnlyRepositoryBuilder GetById(Expense expense)
    {
        _mock.Setup(repository => repository.GetById(It.IsAny<Guid>())).ReturnsAsync(expense);

        return this;
    }

    public IExpenseUpdateOnlyRepository Build() => _mock.Object;
}