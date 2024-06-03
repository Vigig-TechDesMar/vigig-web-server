using Vigig.Domain.Models;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class ClaimedVoucher : BaseEntity<Guid>
{
    public required string EventTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid VoucherId { get; set; }
    public Guid CustomerId { get; set; }
    public virtual required Voucher Voucher { get; set; }
    public virtual required VigigUser Customer { get; set; }
}
