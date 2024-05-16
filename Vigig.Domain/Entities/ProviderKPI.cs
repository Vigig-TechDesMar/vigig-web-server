using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class ProviderKPI : BaseEntity<Guid>
{
    public float Progress { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid ProviderId { get; set; }
    public Guid LeaderBoardId { get; set; }
    public virtual required Provider Provider { get; set; }
    public virtual required  LeaderBoard LeaderBoard { get; set; }
}
