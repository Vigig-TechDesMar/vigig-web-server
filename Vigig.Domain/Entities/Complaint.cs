using Vigig.Domain.Enums;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Complaint : BaseEntity<Guid>
{
    public string Content { get; set; } = string.Empty;
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
    public Guid? BookingId { get; set; }
    public Guid ComplaintTypeId { get; set; }
    public virtual Booking? Booking { get; set; }
    public virtual required ComplaintType ComplaintType { get; set; } 
}