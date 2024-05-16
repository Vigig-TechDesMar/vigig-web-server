using Vigig.Domain.Models.BaseEntities;
using Vigig.Domain.Entities;

namespace Vigig.Domain.Entities;

public partial class Provider : IdentityEntity
{
    public double? Rating { get; set; }
    
    public DateTime? ExpirationPlanDate { get; set; }
    
    public Guid BuildingId { get; set; }
    
    public Guid BadgeId { get; set; }

    public virtual Badge Badge { get; set; } = null!;

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<Deposit> Deposits { get; set; } = Array.Empty<Deposit>();

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = Array.Empty<ProviderService>();

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = Array.Empty<SubscriptionFee>();

    public virtual ICollection<Wallet> Wallets { get; set; } = Array.Empty<Wallet>();

    public virtual ICollection<ProviderKPI> KPIs { get; set; } = Array.Empty<ProviderKPI>();
}
