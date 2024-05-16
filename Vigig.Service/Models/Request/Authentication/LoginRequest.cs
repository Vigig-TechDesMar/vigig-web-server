using System.ComponentModel.DataAnnotations;
using Vigig.Service.Enums;

namespace Vigig.Service.Models.Request.Authentication;

public class LoginRequest
{
    [Required]
    public required string Email { get; set; }
    
    [Required]
    public required string Password { get; set; } 

}