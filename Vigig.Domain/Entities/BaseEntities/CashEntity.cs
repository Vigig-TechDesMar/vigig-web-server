using Vigig.Domain.Enums;

namespace Vigig.Domain.Entities.BaseEntities;

public class CashEntity
{
    public Guid Id { get; set; }
    public required double Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public CashStatus Status { get; set; } = CashStatus.Pending;
}