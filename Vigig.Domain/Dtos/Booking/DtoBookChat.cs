namespace Vigig.Domain.Dtos.Booking;

public class DtoBookChat
{
    public Guid Id { get; set; }
    public required string ClientName { get; set; }
    public required string ProviderName { get; set; }
    public required string ClientProfileImage { get; set; }
    public required string ProviderProfileImage { get; set; }
    public required string LastMessage { get; set; } = String.Empty;

    public required string ChatTitle { get; set; } = String.Empty;
}