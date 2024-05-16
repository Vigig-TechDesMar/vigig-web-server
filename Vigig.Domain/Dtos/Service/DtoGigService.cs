namespace Vigig.Domain.Dtos.Service;

public class DtoGigService
{
    public Guid Id { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public double MinPrice { get; set; }

    public double MaxPrice { get; set; }

    public double Fee { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }
    
    public Guid ServiceCategoryId { get; set; }
}