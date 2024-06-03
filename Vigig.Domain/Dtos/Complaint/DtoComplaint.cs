namespace Vigig.Domain.Dtos.Complaint;

public class DtoComplaint
{
    public Guid Id;
    
    public Guid BookingId { get; set; }

    public Guid ComplaintTypeId { get; set; }
    
    public bool IsActive { get; set; } 

}