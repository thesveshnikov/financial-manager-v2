namespace FinancialManager.Domain.Repositories.RefreshToken;

public interface IRefreshTokenReadOnlyRepository
{
    Task<Entities.RefreshToken?> Get(string refreshToken);
}