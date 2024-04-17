using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Deposit
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime MadeDate { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public Guid ProviderId { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
