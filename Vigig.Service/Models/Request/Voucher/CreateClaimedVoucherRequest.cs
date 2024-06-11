using Vigig.Service.Constants;

namespace Vigig.Service.Models.Request.Voucher;

public class CreateClaimedVoucherRequest
{

    public Domain.Entities.Badge[]? Badges { get; set; }

    public bool IsForAll { get; set; } = false;
    public Guid VoucherId { get; set; }
    
    public Guid? CustomerId { get; set; }
}