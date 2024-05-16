﻿using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class Wallet
{
    public Guid Id { get; set; }

    public double? Balance { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid ProviderId { get; set; }

    public virtual VigigUser Provider { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
