using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class GigService
{
    public Guid Id { get; set; }

    public required string ServiceName { get; set; } 

    public string? Description { get; set; }

    public double MinPrice { get; set; }

    public double MaxPrice { get; set; }

    public double Fee { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid ServiceCategoryId { get; set; }

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = Array.Empty<ProviderService>();

    public virtual ServiceCategory ServiceCategory { get; set; } = null!;
}
