namespace Vigig.Domain.Dtos.Event;

public class DtoBanner
{
    public Guid Id;

    public string AltText { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}