using Microsoft.AspNetCore.Http;
using Vigig.Common.Constants;

namespace Vigig.Service.Models.Common;

public class ServiceActionResult
{
    public object? Data { get; set; }
    public bool IsSuccess { get; set; }
    
    public string? Detail { get; set; }
    
    public int StatusCode { get; set; }

    public ServiceActionResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
    public ServiceActionResult(bool isSuccess, string detail)
    {
        IsSuccess = isSuccess;
        Detail = detail;
    }

    public ServiceActionResult() : this(true)
    {
    }
}