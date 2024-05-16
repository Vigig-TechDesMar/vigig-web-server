using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Notification : BaseEntity<Guid>
{
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Guid NotificationTypeId { get; set; }

    public required NotificationType NotificationType { get; set; } 
}