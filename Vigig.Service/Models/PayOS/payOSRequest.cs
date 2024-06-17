using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.PayOS;

public class payOSRequest
{
    [Required]
    public required string Code { get; set; }
    
    [Required]
    public required string Desc { get; set; }
    
    [Required]
    public required object Data { get; set; }
    
    [Required]
    public required string Signature { get; set; }
}