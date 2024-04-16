namespace Vigig.Service.Models;

public class ServiceActionResult
{
    public object? Data { get; set; }
    public bool IsSuccess { get; set; }
    
    public string? Detail { get; set; }

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