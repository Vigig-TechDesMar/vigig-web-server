using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Building
{
    public Guid Id { get; set; }

    public string BuildingName { get; set; } = null!;

    public string? Note { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
