using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class SubscriptionPlan
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public int? DurationType { get; set; }

    public double? Price { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = new List<SubscriptionFee>();
}
