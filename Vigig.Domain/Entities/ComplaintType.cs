using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class ComplaintType : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
}