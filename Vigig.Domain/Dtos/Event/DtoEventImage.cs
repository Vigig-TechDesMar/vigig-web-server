namespace Vigig.Domain.Dtos.Event;

public class DtoEventImage
{
    public Guid Id;
    
    public string ImageUrl { get; set; } = null!;
    
    public Guid? PopUpId { get; set; }
    
    public Guid? BannerId { get; set; }
}