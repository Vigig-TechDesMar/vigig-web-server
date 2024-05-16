using System;
using System.Collections.Generic;

namespace Vigig.Domain.Entities;

public partial class Building
{
    public Guid Id { get; set; }

    public required string BuildingName { get; set; } 

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = Array.Empty<Booking>();

    public virtual ICollection<VigigUser> Users { get; set; } = Array.Empty<VigigUser>();

}
