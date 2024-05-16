using System.Collections;
using Microsoft.AspNetCore.Routing.Constraints;
using Vigig.Domain.Entities;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public partial class VigigUser : IdentityEntity
{
    public Guid BuildingId { get; set; }
    
    public Guid? BadgeId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = Array.Empty<Booking>();

    public virtual required Building Building { get; set; } 

    public virtual Badge? Badge { get; set; } 

    public virtual ICollection<VigigRole> Roles { get; set; } = Array.Empty<VigigRole>();

    public virtual ICollection<ClaimedVoucher> ClaimedVouchers { get; set; } = Array.Empty<ClaimedVoucher>();

    public virtual ICollection<Wallet> Wallets { get; set; } = Array.Empty<Wallet>();

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = Array.Empty<SubscriptionFee>();

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = Array.Empty<ProviderService>();

    public virtual ICollection<ProviderKPI> KPIs { get; set; } = Array.Empty<ProviderKPI>();

    public virtual ICollection<Deposit> Deposits { get; set; } = Array.Empty<Deposit>();
}
