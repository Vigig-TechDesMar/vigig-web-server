using Vigig.Domain.Models;
using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class ClaimedVoucher : BaseEntity<Guid>
{
    public required string EventTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public required string Field { get; set; }
    public Guid VoucherId { get; set; }
    public Guid CustomerId { get; set; }
    public required virtual Voucher Voucher { get; set; }
    public required virtual VigigUser Customer { get; set; }
}
