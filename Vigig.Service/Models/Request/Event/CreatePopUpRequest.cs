using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Event;

public class CreatePopUpRequest
{
    [Required]
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}