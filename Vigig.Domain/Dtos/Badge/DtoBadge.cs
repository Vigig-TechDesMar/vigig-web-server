namespace Vigig.Domain.Dtos.Badge;

public class DtoBadge
{
    public Guid Id { get; set; }

    public string BadgeName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Benefit { get; set; }
}