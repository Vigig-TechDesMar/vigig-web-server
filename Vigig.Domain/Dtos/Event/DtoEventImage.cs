namespace Vigig.Domain.Dtos.Event;

public class DtoEventImage
{
    public Guid Id;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public string ImageUrl { get; set; } = null!;
    
    public Guid EventId { get; set; }
}