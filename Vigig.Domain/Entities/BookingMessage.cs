using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class BookingMessage
{
    public Guid Id { get; set; }

    public string SenderName { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public Guid BookingId { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
