using Vigig.Domain.Entities;

namespace Vigig.Service.Models.Common;

public class AuthModel
{
    public Guid UserId { get; set; }
    public required string Role { get; set; } 
    public required string UserName { get; set; }
}