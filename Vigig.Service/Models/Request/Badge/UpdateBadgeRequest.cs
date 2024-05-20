using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Badge;

public class UpdateBadgeRequest
{
    public Guid Id { get; set; }
    
    [Required]
    public required string BadgeName { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public required string Benefit { get; set; }
}