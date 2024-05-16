using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class Booking
{
    public Guid Id { get; set; }

    public required string Apartment { get; set; } 

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

    public virtual ICollection<BookingFee> BookingFees { get; set; } =  Array.Empty<BookingFee>();

    public virtual ICollection<BookingMessage> BookingMessages { get; set; } = Array.Empty<BookingMessage>();

    public virtual Building Building { get; set; } = null!;

    public virtual VigigUser VigigUser { get; set; } = null!;

    public virtual ProviderService ProviderService { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = Array.Empty<Complaint>();
}
