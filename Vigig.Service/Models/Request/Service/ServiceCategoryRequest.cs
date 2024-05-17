using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.GigService;

public class ServiceCategoryRequest
{
    [Required] 
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
}