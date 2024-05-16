﻿namespace Vigig.Domain.Entities;

public partial class ServiceImage
{
    public Guid Id { get; set; }

    public required string ImageUrl { get; set; } 

    public Guid ProviderServiceId { get; set; }

    public virtual required ProviderService ProviderService { get; set; }
}
