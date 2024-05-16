using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class UserToken 
{
    public Guid UserId { get; set; }
    public virtual required string LoginProvider { get; set; } 
    public virtual required string Name { get; set; } 
    public virtual required string Value { get; set; } 
}