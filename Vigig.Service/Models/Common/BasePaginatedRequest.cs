using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Models.Common;

public class BasePaginatedRequest
{
    [Range(1,int.MaxValue)]
    public int PageSize { get; set; }
    [Range(1,int.MaxValue)]
    public int PageIndex { get; set; }
}