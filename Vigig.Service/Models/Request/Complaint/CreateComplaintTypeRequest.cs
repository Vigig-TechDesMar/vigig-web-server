using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Complaint;

public class CreateComplaintTypeRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = String.Empty;
}