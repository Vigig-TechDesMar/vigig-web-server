using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class PopUp : BaseEntity<Guid>
{
    public required string Title { get; set; }
    public string? SubTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid EventId { get; set; }
    public required virtual Event Event { get; set; }
}
