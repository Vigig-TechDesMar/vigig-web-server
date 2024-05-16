using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class UserToken 
{
    public Guid UserId { get; set; }
    public virtual string LoginProvider { get; set; } = null!;
    public virtual string Name { get; set; } = null!;
    public virtual string Value { get; set; } = null!;
}