namespace Vigig.Domain.Dtos.Badge;

public class DtoBadgeWithStatus
{
    public Guid Id { get; set; }

    public string BadgeName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Benefit { get; set; }
    
    public bool IsActive { get; set; }
}