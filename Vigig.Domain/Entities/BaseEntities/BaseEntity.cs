namespace Vigig.Domain.Models.BaseEntities;

public abstract class BaseEntity<Tkey>
{
    public  Tkey Id { get; set; }
}
