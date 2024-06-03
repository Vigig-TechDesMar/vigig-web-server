namespace Vigig.Domain.Dtos.Booking;

public class DtoBookingMessage
{
    public Guid Id { get; set; }

    public required string SenderName { get; set; }

    public required string Content { get; set; } 

    public DateTime SentAt { get; set; }
    
    public required string SenderProfileImage { get; set; }
}