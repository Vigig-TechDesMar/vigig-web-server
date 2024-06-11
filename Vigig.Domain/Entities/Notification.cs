using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Notification : BaseEntity<Guid>
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Guid NotificationTypeId { get; set; }
    public Guid? UserId { get; set; }

    public VigigUser VigigUser { get; set; } = null!;
    public NotificationType NotificationType { get; set; } = null!;
}