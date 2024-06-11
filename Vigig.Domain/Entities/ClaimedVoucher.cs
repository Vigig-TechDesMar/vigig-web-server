using Vigig.Domain.Models;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class ClaimedVoucher
{
    public required string EventTitle { get; set; }

    public DateTime? UsedDate { get; set; }
    public bool IsUsed { get; set; } = false;
    public Guid VoucherId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual required Voucher Voucher { get; set; }
    public virtual required VigigUser Customer { get; set; }
}
