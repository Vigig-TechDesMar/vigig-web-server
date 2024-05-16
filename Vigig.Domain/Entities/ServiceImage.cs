namespace Vigig.Domain.Entities;

public partial class ServiceImage
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public Guid ProviderServiceId { get; set; }

    public virtual required ProviderService ProviderService { get; set; }
}
