using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public class VigigRole : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public virtual ICollection<VigigUser> VigigUsers { get; set; } = new List<VigigUser>();
}