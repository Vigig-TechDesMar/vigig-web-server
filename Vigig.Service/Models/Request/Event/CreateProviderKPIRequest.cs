using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Event;

public class CreateProviderKPIRequest
{
    public float Progress { get; set; } = 0;
    
    public Guid ProviderId { get; set; }
    
    public Guid LeaderBoardId { get; set; }
}