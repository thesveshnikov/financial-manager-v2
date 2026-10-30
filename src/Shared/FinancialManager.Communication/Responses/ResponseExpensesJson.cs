namespace FinancialManager.Communication.Responses;

public class ResponseExpensesJson
{
    public IList<ResponseExpenseJson> Expenses { get; set; } = [];
}