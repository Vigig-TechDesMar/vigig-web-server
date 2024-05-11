using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class ServiceCategory
{
    public Guid Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<GigService> GigServices { get; set; } = new List<GigService>();
}
