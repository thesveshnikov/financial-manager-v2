using System.Runtime.CompilerServices;
using FinancialManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("WebApi.Tests")]
namespace FinancialManager.Infrastructure.DataAccess;

public class FinancialManagerDbContext : DbContext
{
    public FinancialManagerDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}