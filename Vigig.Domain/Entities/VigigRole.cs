using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class VigigRole : BaseEntity<Guid>
{
    public required string Name { get; set; } 
    public required string NormalizedName { get; set; } 
    public virtual ICollection<VigigUser> VigigUsers { get; set; } = new List<VigigUser>();
}
