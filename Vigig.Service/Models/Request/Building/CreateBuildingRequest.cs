using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Request.Building;

public class CreateBuildingRequest
{
    [Required] 
    public string BuildingName { get; set; } = null!;
    public string? Note { get; set; } = String.Empty;
}