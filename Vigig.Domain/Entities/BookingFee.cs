using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class BookingFee
{
    public Guid Id { get; set; }

    public double? Amount { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid BookingId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
