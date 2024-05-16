using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class ServiceImage
{
    public Guid Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public Guid ProviderServiceId { get; set; }

    public virtual ProviderService ProviderService { get; set; } = null!;
}
