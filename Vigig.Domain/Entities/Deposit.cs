using System;
using System.Collections.Generic;
using Vigig.Domain.Entities.BaseEntities;

namespace Vigig.Domain.Entities;

public partial class Deposit : CashEntity
{

    public required string PaymentMethod { get; set; } 

    public Guid ProviderId { get; set; }

    public virtual required VigigUser Provider { get; set; } 

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
