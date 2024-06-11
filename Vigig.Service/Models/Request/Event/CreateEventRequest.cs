using System.ComponentModel.DataAnnotations;
using Vigig.Service.Enums;

namespace Vigig.Service.Models.Request.Event;

public class CreateEventRequest
{
    [Required]
    public required string EventTitle { get; set; }
    
    public string? EventDescription { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }

    //Email
    public bool EmailUser { get; set; }
    public bool IsForAll { get; set; } = false;
    public UserRole[]? Targets { get; set; }

    public string Body { get; set; } = String.Empty;
}