namespace Vigig.Domain.Dtos.Voucher;

public class DtoClaimedVoucher
{
    public Guid Id;

    public string EventTitle { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid VoucherId { get; set; }
    
    public Guid CustomerId { get; set; }
}