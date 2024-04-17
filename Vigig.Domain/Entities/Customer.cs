using System;
using System.Collections.Generic;

namespace Vigig.Domain.Models;

public partial class Customer
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Gender { get; set; }

    public string? ProfileImage { get; set; }

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public Guid BuildingId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Building Building { get; set; } = null!;
}
