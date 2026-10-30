using FinancialManager.Communication.Enums;

namespace FinancialManager.Communication.Responses;

public class ResponseExpenseJson
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
    public Enums.Type Type { get; set; }
    public DateTime ExpenseDate { get; set; }
}