using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Complaint;

public class UpdateComplaintTypeRequest
{
    public Guid Id;
    
    [Required]
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = String.Empty;

}