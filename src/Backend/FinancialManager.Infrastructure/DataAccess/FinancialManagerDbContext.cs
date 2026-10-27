using FinancialManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialManager.Infrastructure.DataAccess;

internal class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

    public DbSet<User> Users {  get; set; }
}