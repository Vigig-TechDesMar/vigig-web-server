using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class BookingMessage
{
    public Guid Id { get; set; }
    
    public required Guid SenderId { get; set; }

    public required string SenderName { get; set; }

    public required string Content { get; set; } 

    public DateTime SentAt { get; set; }

    public Guid BookingId { get; set; }

    public virtual required Booking Booking { get; set; } 
}
