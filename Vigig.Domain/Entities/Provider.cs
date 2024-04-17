using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Provider
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public string? ProfileImage { get; set; }

    public double? Rating { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public bool? EmailConfirmed { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? ExpirationPlanDate { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid BuildingId { get; set; }

    public Guid BadgeId { get; set; }

    public virtual Badge Badge { get; set; } = null!;

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();

    public virtual ICollection<SubscriptionFee> SubscriptionFees { get; set; } = new List<SubscriptionFee>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
