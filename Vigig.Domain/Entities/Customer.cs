using System;
using System.Collections.Generic;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public partial class Customer : IdentityEntity
{
    public Guid BuildingId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Building Building { get; set; } = null!;
}
