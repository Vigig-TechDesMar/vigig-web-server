namespace Vigig.Domain.Dtos.Event;

public class DtoPopUp
{
    public Guid Id;
    
    public string Title { get; set; } = null!;
    
    public string? SubTitle { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public Guid EventId { get; set; }
}