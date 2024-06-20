using Vigig.Domain.Models.BaseEntities;

namespace Vigig.Domain.Entities;

public class Voucher 
{
    public Guid Id { get; set; }
    public required string VoucherTitle { get; set; }
    public string? IconUrl { get; set; }
    public int? Amount { get; set; }
    public int? MaxAmount { get; set; }
    public required string Content { get; set; }
    public double Percentage { get; set; }
    public int Limit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public Guid EventId { get; set; }
    public required Event Event { get; set; }
    public virtual ICollection<ClaimedVoucher> ClaimedVouchers { get; set; } = new List<ClaimedVoucher>();
}
