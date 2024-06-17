using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Complaint;

public class CreateComplaintRequest
{
    public Guid? BookingId { get; set; }

    [Required]
    public Guid ComplaintTypeId { get; set; }

    [Required] public string Content { get; set; } = string.Empty;

}