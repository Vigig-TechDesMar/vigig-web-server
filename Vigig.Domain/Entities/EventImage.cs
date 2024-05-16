using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class EventImage : BaseEntity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string ImageUrl { get; set; }
    public required string Field { get; set; }
    public Guid EventId { get; set; }
    public virtual required  Event Event { get; set; }
}
