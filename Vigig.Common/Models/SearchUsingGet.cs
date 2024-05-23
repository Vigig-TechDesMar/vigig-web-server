using System.ComponentModel.DataAnnotations;
using Vigig.Common.Models;

namespace Vigig.Service.Models.Common;

public class SearchUsingGet 
{
    [Range(1,int.MaxValue)]
    public int PageSize { get; set; }
    [Range(1,int.MaxValue)]
    public int PageIndex { get; set; }
    public string? SearchField { get; set; }
    public string? SearchValue { get; set; }
    public string? SortField { get; set; }
    public bool Descending { get; set; }
}