using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class NotificationType : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}