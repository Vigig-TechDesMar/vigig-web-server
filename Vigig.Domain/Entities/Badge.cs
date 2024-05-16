using System;
using System.Collections.Generic;
using Vigig.Domain.Models;

namespace Vigig.Domain.Entities;

public partial class Badge
{
    public Guid Id { get; set; }

    public string BadgeName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Benefit { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<VigigUser> Providers { get; set; } = Array.Empty<VigigUser>();
}
