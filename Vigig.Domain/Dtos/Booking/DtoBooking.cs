﻿namespace Vigig.Domain.Dtos.Booking;

public class DtoBooking : DtoPlacedBooking
{
    public Guid Id { get; set; }
    public double FinalPrice { get; set; }
    
    public double? CustomerRating { get; set; }

    public string? CustomerReview { get; set; }
    public bool IsCancellable { get; set; }
    public string? BookerProfileImage { get; set; } 
    public string? ProviderProfileImage { get; set; }

}