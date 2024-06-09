using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class LeaderBoard : BaseEntity<Guid>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid EventId { get; set; }
    public virtual required Event Event { get; set; }
    public virtual ICollection<ProviderKPI> KPIs { get; set; } = new List<ProviderKPI>();
}
