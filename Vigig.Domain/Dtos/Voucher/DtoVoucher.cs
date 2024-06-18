namespace Vigig.Domain.Dtos.Voucher;

public class DtoVoucher
{
    public Guid Id;
    
    public string Content { get; set; } = null!;
    
    public double Percentage { get; set; }
    
    public int Limit { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; }
    
    public Guid EventId { get; set; }
}