using System.ComponentModel.DataAnnotations;
using Vigig.Common.Constants.Validations;
using Vigig.Service.Attributes;
using Vigig.Service.Enums;

namespace Vigig.Service.Models.Request.Authentication;

public class RegisterRequest
{
    [Required]
    [MatchesPattern(UserProfileValidation.Email.EmailPattern,UserProfileValidation.Email.NotMatchedPatternMessage)]
    public required string Email { get; set; }
    
    [Required]
    [MatchesPattern(UserProfileValidation.Password.PasswordPattern,UserProfileValidation.Password.NotMatchedPatternMessage)]
    public required string Password { get; set; } 

    [Required] 
    public UserRole Role { get; set; } 

}