using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Category;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

public class CategoryRepository : ICategoryWriteOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public CategoryRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Category category) => await _dbContext
        .Categories
        .AddAsync(category);
}