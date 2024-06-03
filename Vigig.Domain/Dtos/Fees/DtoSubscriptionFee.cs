namespace Vigig.Domain.Dtos.Fees;

public class DtoSubscriptionFee
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid ProviderId { get; set; }

    public Guid SubscriptionPlanId { get; set; }

}