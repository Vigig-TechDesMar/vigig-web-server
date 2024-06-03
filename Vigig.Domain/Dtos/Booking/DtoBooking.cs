namespace Vigig.Domain.Dtos.Booking;

public class DtoBooking : DtoPlacedBooking
{
    public double FinalPrice { get; set; }
    
    public double? CustomerRating { get; set; }

    public string? CustomerReview { get; set; }
}