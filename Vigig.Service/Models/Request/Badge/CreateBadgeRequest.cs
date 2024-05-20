using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Badge;

public class CreateBadgeRequest
{
    [Required]
    public required string BadgeName { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public required string Benefit { get; set; }
    
}