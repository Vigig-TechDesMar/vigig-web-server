using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Event;

public class UpdateProviderKPIRequest
{
    public Guid Id;
    
    [Required]
    [MinValue(0)]
    [MaxValue(100)]
    public float Progress { get; set; }
    
    public Guid ProviderId { get; set; }
    
    public Guid LeaderBoardId { get; set; }
}