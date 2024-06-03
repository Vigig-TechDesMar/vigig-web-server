using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Booking;

public class BookingRatingRequest
{
    public Guid BookingId { get; set; }
    [Required]
    [Range(1,5)]
    public double Rating { get; set; }
    
    public string Review { get; set; } = string.Empty;
}