using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class BookingFee
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid BookingId { get; set; }

    public virtual required Booking Booking { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
