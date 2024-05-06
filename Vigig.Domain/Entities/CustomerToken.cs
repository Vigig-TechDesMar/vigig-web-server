using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public class CustomerToken 
{
    public Guid CustomerId { get; set; }
    public virtual string LoginProvider { get; set; } = null!;
    public virtual string Name { get; set; } = null!;
    public virtual string Value { get; set; } = null!;
}