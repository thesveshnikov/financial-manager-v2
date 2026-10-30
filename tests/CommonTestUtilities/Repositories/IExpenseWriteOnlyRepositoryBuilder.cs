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

    public IExpenseWriteOnlyRepository Build() => _mock.Object;
}