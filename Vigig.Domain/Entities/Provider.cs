using System;
using System.Collections.Generic;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public partial class Provider : IdentityEntity
{
    public double? Rating { get; set; }
    
    public DateTime? ExpirationPlanDate { get; set; }
    
    public Guid BuildingId { get; set; }
    
    public Guid BadgeId { get; set; }

    public virtual Badge Badge { get; set; } = null!;

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = new List<SubscriptionFee>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
