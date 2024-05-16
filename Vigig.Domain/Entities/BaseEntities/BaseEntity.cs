namespace Vigig.Domain.Models.BaseEntities;

public abstract class BaseEntity<Tkey>
{
    public required Tkey Id { get; set; }
}
