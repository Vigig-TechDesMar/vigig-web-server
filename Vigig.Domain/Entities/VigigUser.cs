using System;
using System.Collections.Generic;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Models;

public partial class VigigUser : IdentityEntity
{
    public Guid BuildingId { get; set; }
    
    public Guid? BadgeId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Building Building { get; set; } = null!;

    public virtual Badge? Badge { get; set; } 

    public virtual ICollection<VigigRole> Roles { get; set; } = new List<VigigRole>();
}
