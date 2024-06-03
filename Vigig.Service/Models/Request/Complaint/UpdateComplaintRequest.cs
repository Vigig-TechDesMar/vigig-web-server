using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Complaint;

public class UpdateComplaintRequest
{
    public Guid Id;
    
    public Guid? BookingId { get; set; }

    [Required]
    public Guid ComplaintTypeId { get; set; }
    
}