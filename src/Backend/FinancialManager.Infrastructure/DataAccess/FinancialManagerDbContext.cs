using FinancialManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess;

public class FinancialManagerDbContext : DbContext
{
    public FinancialManagerDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users { get; set; }
}