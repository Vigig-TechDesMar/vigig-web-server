using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class Deposit
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime MadeDate { get; set; }

    public required string PaymentMethod { get; set; } 

    public Guid ProviderId { get; set; }

    public virtual required VigigUser Provider { get; set; } 

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
