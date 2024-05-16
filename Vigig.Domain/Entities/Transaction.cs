using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

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

    public virtual required BookingFee BookingFee { get; set; } 
    public virtual required Deposit Deposit { get; set; } 
    public virtual required SubscriptionFee SubscriptionFee { get; set; } 
    public virtual required Wallet Wallet { get; set; } 
}
