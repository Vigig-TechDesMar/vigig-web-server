using Microsoft.AspNetCore.Http;

namespace Vigig.Service.Models.Request.Authentication;

public class UpdateProfileRequest
{
    public string? Phone { get; set; }
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public IFormFile? ProfileImage { get; set; }
    public string? Gender { get; set; }
    public Guid BuildingId { get; set; }
}