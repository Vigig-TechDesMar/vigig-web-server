using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public class VigigRole : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
}