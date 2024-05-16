using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class SubscriptionFee
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid ProviderId { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
