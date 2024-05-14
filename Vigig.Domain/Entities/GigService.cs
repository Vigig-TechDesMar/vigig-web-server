﻿using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class GigService
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

    public virtual ICollection<ProviderService> ProviderServices { get; set; } = new List<ProviderService>();

    public virtual ServiceCategory ServiceCategory { get; set; } = null!;
}