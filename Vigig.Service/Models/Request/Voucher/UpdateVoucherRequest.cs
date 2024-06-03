using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Voucher;

public class UpdateVoucherRequest
{
    public Guid Id;
    
    [Required]
    public required string Content { get; set; }
    
    public float Percentage { get; set; }
    
    public uint Limit { get; set; }
    
    [Required]
    [MinValue(1)]
    public uint Quantity { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}