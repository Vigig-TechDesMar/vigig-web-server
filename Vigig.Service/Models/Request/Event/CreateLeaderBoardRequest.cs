using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Event;

public class CreateLeaderBoardRequest
{
    [Required]
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
    
    //Email
    public bool EmailUser { get; set; }
    public string Body { get; set; } = String.Empty;
}