using FinancialManager.Domain.Repositories;

namespace FinancialManager.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly FinancialManagerDbContext _dbContext;

    public UnitOfWork(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}