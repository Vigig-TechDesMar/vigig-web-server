namespace Vigig.Domain.Dtos.Event;

public class DtoLeaderBoard
{
    public Guid Id;

    public string Name { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}