using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class ProviderService
{
    public Guid Id { get; set; }

    public double Rating { get; set; } = default;

    public int RatingCount { get; set; } = default;

    public double TotalBooking { get; set; } = default;

    public double StickerPrice { get; set; }

    public string? Description { get; set; }

    public bool IsVisible { get; set; } = true;

    public bool IsActive { get; set; }

    public Guid ProviderId { get; set; }

    public Guid ServiceId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual VigigUser Provider { get; set; } = null!;

    public virtual GigService Service { get; set; } = null!;

    public virtual ICollection<ServiceImage> ServiceImages { get; set; } = new List<ServiceImage>();
}
