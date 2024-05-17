using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Building;

public class UpdateBuildingRequest
{
    public Guid Id { get; set; }
    [Required] 
    public string BuildingName { get; set; } = null!;
    public string Note { get; set; } = String.Empty;
}