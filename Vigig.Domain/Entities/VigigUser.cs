using System.Collections;
using Microsoft.AspNetCore.Routing.Constraints;
using Vigig.Domain.Entities;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public partial class VigigUser : IdentityEntity
{
    public Guid BuildingId { get; set; }
    
    public Guid? BadgeId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual required Building Building { get; set; } 

    public virtual Badge? Badge { get; set; } 

    public virtual ICollection<VigigRole> Roles { get; set; } = new List<VigigRole>();

    public virtual ICollection<ClaimedVoucher> ClaimedVouchers { get; set; } = new List<ClaimedVoucher>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = new List<SubscriptionFee>();

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();

    public virtual ICollection<ProviderKPI> KPIs { get; set; } = new List<ProviderKPI>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
}
