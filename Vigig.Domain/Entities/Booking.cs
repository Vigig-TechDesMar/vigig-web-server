using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Booking
{
    public Guid Id { get; set; }

    public string Apartment { get; set; } = null!;

    public double StickerPrice { get; set; }

    public double FinalPrice { get; set; }

    public int Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public double? ProviderRating { get; set; }

    public string? ProviderReview { get; set; }

    public double? CustomerRating { get; set; }

    public string? CustomerReview { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid CustomerId { get; set; }

    public Guid ProviderServiceId { get; set; }

    public Guid BuildingId { get; set; }

    public virtual ICollection<BookingFee> BookingFees { get; set; } = new List<BookingFee>();

    public virtual ICollection<BookingMessage> BookingMessages { get; set; } = new List<BookingMessage>();

    public virtual Building Building { get; set; } = null!;

    public virtual VigigUser VigigUser { get; set; } = null!;

    public virtual ProviderService ProviderService { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
