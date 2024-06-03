using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Event;

namespace Vigig.Service.Interfaces;

[ServiceRegister]
public interface ILeaderBoardService
{
    Task<ServiceActionResult> GetAllAsync();

    Task<ServiceActionResult> GetById(Guid id);
    
    Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request);
    
    Task<ServiceActionResult> SearchLeaderBoard(SearchUsingGet request);

    Task<ServiceActionResult> AddAsync(CreateLeaderBoardRequest request);

    Task<ServiceActionResult> UpdateAsync(UpdateLeaderBoardRequest request);

    Task<ServiceActionResult> DeleteAsync(Guid id);
}