namespace Vigig.Domain.Dtos.SubscriptionPlan;

public class DtoSubscriptionPlan
{
    public Guid Id { get; set; }
    
    public string? Description { get; set; }

    public int? DurationType { get; set; }

    public double? Price { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }
}