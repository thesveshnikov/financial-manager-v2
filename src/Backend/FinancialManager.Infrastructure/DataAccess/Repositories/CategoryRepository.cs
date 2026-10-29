using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.Category;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

public class CategoryRepository : ICategoryWriteOnlyRepository, ICategoryReadOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public CategoryRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(Category category) => await _dbContext
        .Categories
        .AddAsync(category);

    public async Task<IList<Category>> GetAll(Guid userId) => await _dbContext
        .Categories
        .AsNoTracking()
        .Where(category => category.Active && category.UserId.Equals(userId))
        .ToListAsync();
}