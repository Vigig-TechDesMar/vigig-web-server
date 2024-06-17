using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public required double Amount { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Status { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid WalletId { get; set; }

    public Guid? DepositId { get; set; }

    public Guid? BookingFeeId { get; set; }

    public Guid? SubscriptionFeeId { get; set; }

    public virtual BookingFee? BookingFee { get; set; } 
    public virtual Deposit? Deposit { get; set; } 
    public virtual SubscriptionFee? SubscriptionFee { get; set; } 
    public virtual required Wallet Wallet { get; set; } 
}
