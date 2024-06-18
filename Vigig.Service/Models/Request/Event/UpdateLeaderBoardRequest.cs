using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Event;

public class UpdateLeaderBoardRequest
{
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }

    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
    
}