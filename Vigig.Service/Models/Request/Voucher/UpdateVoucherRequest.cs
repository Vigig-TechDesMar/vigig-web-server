using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Voucher;

public class UpdateVoucherRequest
{
    public Guid Id { get; set; }
    
    [Required]
    public required string VoucherTitle { get; set; }
    
    public string? IconUrl { get; set; }
    
    public int? Amount { get; set; }
    
    public int? MaxAmount { get; set; }
    
    [Required]
    public required string Content { get; set; }
    
    public float Percentage { get; set; }
    
    public int Limit { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [LaterThanOrAtTheSameTime("StartDate")]
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
}