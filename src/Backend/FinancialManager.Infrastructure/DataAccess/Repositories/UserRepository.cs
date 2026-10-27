using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.User;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public UserRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
}