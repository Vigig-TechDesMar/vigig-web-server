using Vigig.Service.Models;

namespace Vigig.Service.Interfaces;

public interface IGigServiceService
{
    Task<ServiceActionResult> GetAllAsync();
    Task<ServiceActionResult> GetById(Guid gigId);
    Task<ServiceActionResult> AddAsync();
    Task<ServiceActionResult> UpdateAsync();
    Task<ServiceActionResult> DeactivateAsync(Guid gigId);

}