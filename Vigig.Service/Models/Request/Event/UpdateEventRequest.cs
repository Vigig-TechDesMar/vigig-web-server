using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Event;

public class UpdateEventRequest
{
    public Guid Id { get; set; }

    [Required]
    public required string EventTitle { get; set; }
    
    public string? EventDescription { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
}