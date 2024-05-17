using System.ComponentModel.DataAnnotations;
using Vigig.Service.Models.Request.GigService;

namespace Vigig.Service.Models.Request.Service;

public class UpdateServiceCategoryRequest : ServiceCategoryRequest
{
    [Required]
    public Guid Id { get; set; }
}