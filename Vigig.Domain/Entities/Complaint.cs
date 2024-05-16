using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Complaint : BaseEntity<Guid>
{
    public bool IsActive { get; set; } = true;
    public Guid BookingId { get; set; }
    public Guid ComplaintTypeId { get; set; }
    public virtual Booking Booking { get; set; } = null!;
    public virtual ComplaintType ComplaintType { get; set; } = null!;
}