using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Repositories.RefreshToken;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenWriteOnlyRepository, IRefreshTokenReadOnlyRepository
{
    private readonly FinancialManagerDbContext _dbContext;

    public RefreshTokenRepository(FinancialManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Replace(RefreshToken refreshToken)
    {
        var existingTokens = await _dbContext.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId)
            .ToListAsync();
        
        _dbContext.RefreshTokens.RemoveRange(existingTokens);

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> Get(string refreshToken)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .SingleOrDefaultAsync(token => token.Active && token.Value.Equals(refreshToken));
    }
}