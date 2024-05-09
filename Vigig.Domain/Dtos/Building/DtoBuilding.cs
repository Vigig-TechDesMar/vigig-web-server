namespace Vigig.Domain.Dtos.Building;

public class DtoBuilding
{
    public Guid Id { get; set; }

    public string BuildingName { get; set; } = null!;

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public string? ConcurrencyStamp { get; set; }
}