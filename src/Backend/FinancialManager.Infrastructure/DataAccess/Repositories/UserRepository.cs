using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public UserRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    
    public async Task DeactivateAccount(Guid userId) => await _dbContext
        .Users
        .Where(user => user.Active && user.Id == userId)
        .ExecuteUpdateAsync(setter => setter.SetProperty(user => user.Active, false));

    public async Task DeleteAccount(Guid userId)
    {
        var userToDelete = await _dbContext.Users.SingleOrDefaultAsync(user => user.Active == false && user.Id == userId);

        if (userToDelete is not null)
        {
            var categories = await _dbContext.Categories.Where(category => category.UserId == userId).ToListAsync();
            _dbContext.Categories.RemoveRange(categories);
            
            var expenses = await _dbContext.Expenses.Where(expense => expense.UserId == userId).ToListAsync();
            _dbContext.Expenses.RemoveRange(expenses);
            
            _dbContext.Users.Remove(userToDelete);
        }
    }

    public async Task<bool> ExistsActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Active && user.Email.Equals(email));
    
    public async Task<bool> ExistsActiveUserWithId(Guid id) => await _dbContext.Users.AnyAsync(user => user.Active && user.Id.Equals(id));

    public async Task<User?> GetByEmail(string email) => await _dbContext.Users
        .AsNoTracking()
        .SingleOrDefaultAsync(user => user.Active && user.Email.Equals(email));

    public async Task<User?> GetById(Guid id) => await _dbContext.Users
        .AsNoTracking()
        .SingleOrDefaultAsync(user => user.Active && user.Id.Equals(id));

    public void UpdateProfile(User user)
    {
        _dbContext.Users.Attach(user);

        _dbContext.Entry(user).Property(user => user.Name).IsModified = true;
        _dbContext.Entry(user).Property(user => user.Email).IsModified = true;
    }
    public async Task UpdatePassword(Guid userId, string passwordHash) => await _dbContext
        .Users
        .Where(user => user.Id == userId)
        .ExecuteUpdateAsync(setter => setter.SetProperty(user => user.Password, passwordHash));
}