using FinancialManager.Domain.Enums;

namespace FinancialManager.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
    public Role Role { get; set; } = Role.Admin;
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Expense> Expenses { get; set; } = [];
}