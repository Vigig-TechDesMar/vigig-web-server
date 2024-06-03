namespace Vigig.Domain.Dtos.Voucher;

public class DtoVoucher
{
    public Guid Id;
    
    public string Content { get; set; } = null!;
    
    public float Percentage { get; set; }
    
    public uint Limit { get; set; }
    
    public uint Quantity { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; }
    
    public Guid EventId { get; set; }
}