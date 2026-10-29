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
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .HasConstraintName("FK_Categories_Users_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_Expenses_Users_UserId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(c => c.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .HasConstraintName("FK_Expenses_Categories_CategoryId")
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses");

            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}