namespace Vigig.Domain.Dtos.Event;

public class DtoEvent
{
    public Guid Id;

    public string EventTitle { get; set; } = null!;
    
    public string? EventDescription { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsActive { get; set; }
}