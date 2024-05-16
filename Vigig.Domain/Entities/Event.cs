using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Event : BaseEntity<Guid>
{
    public required string EventTitle { get; set; }
    public string? EventDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<LeaderBoard> LeaderBoards { get; set; } = Array.Empty<LeaderBoard>();
    public virtual ICollection<EventImage> EventImages { get; set; } = Array.Empty<EventImage>();
    public virtual ICollection<Voucher> Vouchers { get; set; } = Array.Empty<Voucher>();
    public virtual ICollection<Banner> Banners { get; set; } = Array.Empty<Banner>();
    public virtual ICollection<PopUp> PopUps { get; set; } = Array.Empty<PopUp>();
}
