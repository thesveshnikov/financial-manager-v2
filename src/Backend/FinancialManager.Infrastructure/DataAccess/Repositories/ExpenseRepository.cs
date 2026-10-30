using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Expense;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

public class ExpenseRepository : IExpenseWriteOnlyRepository, IExpenseReadOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public ExpenseRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense) => await _dbContext
        .Expenses
        .AddAsync(expense);

    public async Task<IList<Expense>> GetAll(Guid userId) => await _dbContext
        .Expenses
        .AsNoTracking()
        .Where(expense => expense.UserId == userId)
        .ToListAsync();
}