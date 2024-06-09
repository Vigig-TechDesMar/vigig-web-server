using Vigig.Domain.Enums;

namespace Vigig.Domain.Dtos.Booking;

public class DtoPlacedBooking
{
    public required string BookerName { get; set; }
    public required string BookerPhone { get; set; }
    public required string Apartment { get; set; }
    public required string BuildingName { get; set; }
    public required string ProviderName { get; set; }
    public required string ProviderServiceName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Status { get; set; }
    
}