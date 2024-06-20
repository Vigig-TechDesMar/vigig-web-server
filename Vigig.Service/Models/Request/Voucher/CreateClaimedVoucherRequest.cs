using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Voucher;

public class CreateClaimedVoucherRequest
{
    public Guid VoucherId { get; set; }
    
    public Guid? CustomerId { get; set; }
    
    public Guid[]? BadgeIds { get; set; }

    public bool IsForAll { get; set; } = false;
}