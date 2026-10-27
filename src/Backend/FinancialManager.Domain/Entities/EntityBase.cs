namespace FinancialManager.Domain.Entities;

public class EntityBase
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public bool Active { get; set; } = true;
}