using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Event;

public class UpdateProviderKPIRequest
{
    public Guid Id { get; set; }
    
    [Required]
    [MinValue(0)]
    [MaxValue(1000)]
    public float Progress { get; set; }
    
    public Guid ProviderId { get; set; }
    
    public Guid LeaderBoardId { get; set; }
}