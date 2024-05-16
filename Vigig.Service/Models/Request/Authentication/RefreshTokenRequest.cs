using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Authentication;

public class RefreshTokenRequest
{
    [Required] 
    public required string RefreshToken { get; set; } 
}