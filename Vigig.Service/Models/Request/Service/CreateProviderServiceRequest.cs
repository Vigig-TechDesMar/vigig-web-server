using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Vigig.Service.Models.Request.Service;

public class CreateProviderServiceRequest
{
    [Required]
    public Guid ServiceId { get; set; }
    [Required]
    public double StickerPrice { get; set; }
    [Required]
    public required string Description { get; set; }

    [Required] 
    public required List<IFormFile> Images { get; set; } 
}