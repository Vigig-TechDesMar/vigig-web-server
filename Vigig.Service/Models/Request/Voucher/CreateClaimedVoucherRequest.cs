namespace Vigig.Service.Models.Request.Voucher;

public class CreateClaimedVoucherRequest
{
    public string EventTitle { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid VoucherId { get; set; }
    
    public Guid CustomerId { get; set; }
}