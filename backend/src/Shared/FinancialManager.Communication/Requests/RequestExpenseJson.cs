using FinancialManager.Communication.Enums;

namespace FinancialManager.Communication.Requests;

public class RequestExpenseJson
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
    public Enums.Type Type { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Guid CategoryId { get; set; }
}