using Vigig.Domain.Entities;

namespace Vigig.Domain.Dtos.Service;

public class DtoProviderService
{
    public Guid Id { get; set; }
    public required string ProviderName { get; set; }
    public required string ServiceName { get; set; }    
    public string? Description { get; set; }
    public double StickerPrice { get; set; }
    public double TotalBooking { get; set; }
    public double Rate { get; set; }
    public ICollection<DtoServiceImage> ServiceImages { get; set; } = new List<DtoServiceImage>();
}