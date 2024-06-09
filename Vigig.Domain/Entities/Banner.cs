using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Banner : BaseEntity<Guid>
{
    public required string AltText { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; }
    public Guid EventId { get; set; }
    public  virtual required Event Event { get; set; }

    public ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
}
