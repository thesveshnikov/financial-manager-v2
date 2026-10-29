using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Expense;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

public class ExpenseRepository : IExpenseWriteOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public ExpenseRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense) => await _dbContext
        .Expenses
        .AddAsync(expense);
}