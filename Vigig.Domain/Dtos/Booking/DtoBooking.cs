namespace Vigig.Domain.Dtos.Booking;

public class DtoBooking : DtoPlacedBooking
{
    public Guid Id { get; set; }
    public double FinalPrice { get; set; }
    
    public double? CustomerRating { get; set; }

    public string? CustomerReview { get; set; }
}