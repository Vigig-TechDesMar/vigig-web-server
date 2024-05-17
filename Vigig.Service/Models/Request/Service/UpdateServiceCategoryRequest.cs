using System.ComponentModel.DataAnnotations;
using Vigig.Service.Models.Request.GigService;

namespace Vigig.Service.Models.Request.Service;

public class UpdateServiceCategoryRequest 
{
    [Required]
    public Guid Id { get; set; }
    [Required] 
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = String.Empty;
}