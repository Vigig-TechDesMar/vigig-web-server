using Vigig.Common.Attribute;
using Vigig.Domain.Dtos.Booking;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Booking;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IBookingService
{
    Task<ServiceActionResult> PlaceBookingAsync(string token, BookingPlaceRequest request);

    Task<DtoPlacedBooking> RetrievedPlaceBookingAsync(string token, BookingPlaceRequest request);

    Task<ServiceActionResult> AcceptBookingAsync(Guid id, string token);

    Task<ServiceActionResult> DeclineBookingAsync(Guid id, string token);

    Task<ServiceActionResult> CancelBookingByClientAsync(Guid id, string token);

    Task<ServiceActionResult> CancelBookingByProviderAsync(Guid id, string token);

    Task<ServiceActionResult> CompleteBookingAsync(Guid id, BookingCompleteRequest request, string token);

    Task<ServiceActionResult> LoadOwnChatBookingAsync(string token);

    Task<ServiceActionResult> RatingBookingAsync(string token, Guid bookingId,BookingRatingRequest request);

    Task<ServiceActionResult> LoadAllBookingsAsync(string token, string status);
}