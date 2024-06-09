using System;
using System.Collections.Generic;
using Vigig.Domain.Enums;

namespace Vigig.Domain.Entities;

public partial class Booking
{
    public Guid Id { get; set; }

    public required string Apartment { get; set; } 

    public double StickerPrice { get; set; }

    public double FinalPrice { get; set; }
    
    public required string BookerName { get; set; }
    
    public required string BookerPhone { get; set; } 
    public BookingStatus Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public double? CustomerRating { get; set; }

    public string? CustomerReview { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid CustomerId { get; set; }

    public Guid ProviderServiceId { get; set; }

    public Guid BuildingId { get; set; }

    public virtual ICollection<BookingFee> BookingFees { get; set; } =  new List<BookingFee>();

    public virtual ICollection<BookingMessage> BookingMessages { get; set; } = new List<BookingMessage>();

    public virtual Building Building { get; set; } = null!;

    public virtual VigigUser VigigUser { get; set; } = null!;

    public virtual ProviderService ProviderService { get; set; } = null!;

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}
