using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Status { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid WalletId { get; set; }

    public Guid DepositId { get; set; }

    public Guid BookingFeeId { get; set; }

    public Guid SubscriptionFeeId { get; set; }

    public virtual BookingFee BookingFee { get; set; } = null!;

    public virtual Deposit Deposit { get; set; } = null!;

    public virtual SubscriptionFee SubscriptionFee { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
