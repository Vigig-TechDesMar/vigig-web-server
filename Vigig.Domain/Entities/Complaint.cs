using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Complaint : BaseEntity<Guid>
{
    public required bool IsActive { get; set; } 
    public Guid? BookingId { get; set; }
    public Guid ComplaintTypeId { get; set; }
    public virtual  Booking Booking { get; set; } 
    public virtual required ComplaintType ComplaintType { get; set; } 
}