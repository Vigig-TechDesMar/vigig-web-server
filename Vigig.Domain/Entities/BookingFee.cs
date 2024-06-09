using System;
using System.Collections.Generic;
using Vigig.Domain.Entities.BaseEntities;

namespace Vigig.Domain.Entities;

public partial class BookingFee : CashEntity
{
    public Guid BookingId { get; set; }

    public virtual required Booking Booking { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
