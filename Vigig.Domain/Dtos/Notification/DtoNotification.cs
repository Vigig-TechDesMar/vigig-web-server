namespace Vigig.Domain.Dtos.Notification;

public class DtoNotification
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? RedirectUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}