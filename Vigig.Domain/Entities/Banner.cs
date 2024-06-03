using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Banner : BaseEntity<Guid>
{
    public required string AltText { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid EventId { get; set; }
    public required virtual Event Event { get; set; }
}
