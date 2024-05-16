namespace Vigig.Service.Models.Response.Authentication;

public class RegisterResponse
{
    public required string Email { get; set; } 
    public required string UserName { get; set; } 
    public DateTime CreatedDate { get; set; }
}