using Vigig.Domain.Entities.BaseEntities;

namespace Vigig.Domain.Entities;

public partial class SubscriptionFee : CashEntity
{

    public Guid ProviderId { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public virtual required VigigUser Provider { get; set; } 

    public virtual required SubscriptionPlan SubscriptionPlan { get; set; } 

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
