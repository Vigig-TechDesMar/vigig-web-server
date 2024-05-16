namespace Vigig.Domain.Entities;

public partial class ServiceCategory
{
    public Guid Id { get; set; }

    public required string CategoryName { get; set; } 

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<GigService> GigServices { get; set; } = new List<GigService>();
}
