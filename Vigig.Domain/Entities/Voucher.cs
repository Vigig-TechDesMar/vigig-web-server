using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Voucher : BaseEntity<Guid>
{
    public required string Content { get; set; }
    public float Percentage { get; set; }
    public uint Limit { get; set; }
    public uint Quantity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid EventId { get; set; }
    public required Event Event { get; set; }
    public virtual ICollection<ClaimedVoucher> ClaimedVouchers { get; set; } = Array.Empty<ClaimedVoucher>();
}
