namespace Vigig.Service.Models.Request.Notification;

public class CreateEventNotificationRequest
{
    public string? Content { get; set; }
    public string? RedirectUrl { get; set; }
}