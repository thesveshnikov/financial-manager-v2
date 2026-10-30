using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Expense;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IExpenseWriteOnlyRepositoryBuilder
{
    private readonly Mock<IExpenseWriteOnlyRepository> _mock;

    public IExpenseWriteOnlyRepositoryBuilder()
    {
        _mock = new Mock<IExpenseWriteOnlyRepository>();
    }

    public IExpenseWriteOnlyRepositoryBuilder Delete(Expense expense)
    {
        _mock.Setup(repository => repository.Delete(expense.Id)).ReturnsAsync(true);
        
        return this;
    }

    public IExpenseWriteOnlyRepository Build() => _mock.Object;
}