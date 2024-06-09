using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class EventImage : BaseEntity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string ImageUrl { get; set; }
    
    public Guid PopUpId { get; set; }
    public PopUp PopUp { get; set; } = null!;
    
    public Guid BannerId { get; set; }
    public Banner Banner { get; set; } = null!;
}
