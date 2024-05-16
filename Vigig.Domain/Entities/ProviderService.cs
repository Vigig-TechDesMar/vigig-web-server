using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class ProviderService
{
    public Guid Id { get; set; }

    public double? Rating { get; set; }

    public double? TotalBooking { get; set; }

    public double? StickerPrice { get; set; }

    public string? Description { get; set; }

    public bool? IsAvailable { get; set; }

    public bool? IsVisible { get; set; }

    public bool? IsActive { get; set; }

    public Guid ProviderId { get; set; }

    public Guid ServiceId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = Array.Empty<Booking>();

    public virtual VigigUser Provider { get; set; } = null!;

    public virtual GigService Service { get; set; } = null!;

    public virtual ICollection<ServiceImage> ServiceImages { get; set; } = Array.Empty<ServiceImage>();
}
