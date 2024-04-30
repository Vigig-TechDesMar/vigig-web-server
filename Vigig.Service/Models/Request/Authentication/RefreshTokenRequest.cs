using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Authentication;

public class RefreshTokenRequest
{
    [Required] public string RefreshToken { get; set; } = null!;
}