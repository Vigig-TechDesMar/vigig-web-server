using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Booking;

public class BookingPlaceRequest
{
    public Guid BuildingId { get; set; }
    public Guid ProviderServiceId { get; set; }
    [Required]
    public required string BookerName { get; set; }
    [Required]
    public required string BookerPhone { get; set; }
    [Required]
    public required string Apartment { get; set; }
}