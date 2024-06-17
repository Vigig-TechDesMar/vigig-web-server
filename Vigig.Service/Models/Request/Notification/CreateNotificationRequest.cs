namespace Vigig.Service.Models.Request.Notification;

public class CreateBookingNotificationRequest
{
    public Guid BookingId { get; set; }
    public string? Content { get; set; }
    public string? RedirectUrl { get; set; }
}