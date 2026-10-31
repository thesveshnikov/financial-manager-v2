using FinancialManager.Domain.Enums;

namespace FinancialManager.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Expense> Expenses { get; set; } = [];
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
}