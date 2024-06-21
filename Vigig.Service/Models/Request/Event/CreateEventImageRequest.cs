namespace Vigig.Service.Models.Request.Event;

public class CreateEventImageRequest
{
    public required string ImageUrl { get; set; }
    
    public Guid? PopUpId { get; set; }
    
    public Guid? BannerId { get; set; }
}