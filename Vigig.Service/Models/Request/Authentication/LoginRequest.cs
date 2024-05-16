using System.ComponentModel.DataAnnotations;
using Vigig.Service.Enums;

namespace Vigig.Service.Models.Request.Authentication;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;

}