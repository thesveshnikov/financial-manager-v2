using FinancialManager.Domain.Enums;

namespace FinancialManager.Domain.Entities;

public class Expense : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
    public Enums.Type Type { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Category? Category { get; set; } = null!;
    public Guid? CategoryId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
}