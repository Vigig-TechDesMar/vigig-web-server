using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Event;

public class UpdateBannerRequest
{
    public Guid Id;

    [Required]
    public required string AltText { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}