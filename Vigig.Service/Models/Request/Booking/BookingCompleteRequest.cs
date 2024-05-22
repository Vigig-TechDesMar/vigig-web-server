namespace Vigig.Service.Models.Request.Booking;

public class BookingCompleteRequest
{
    public double FinalPrice { get; set; }
    public double CustomerRating { get; set; }
    public string? CustomerReview { get; set; }
}