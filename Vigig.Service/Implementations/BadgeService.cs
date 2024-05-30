using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos;
using Vigig.Domain.Dtos.Badge;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Badge;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Service.Implementations;

public class BadgeService : IBadgeService
{
    private readonly IBadgeRepository _badgeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BadgeService(IBadgeRepository badgeRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _badgeRepository = badgeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> AddAsync(CreateBadgeRequest request)
    {
        var badge = _mapper.Map<Badge>(request);
        await _badgeRepository.AddAsync(badge);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBadge>(badge),
            StatusCode = StatusCodes.Status201Created
        };

    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var badges = _mapper.ProjectTo<DtoBadgeWithStatus>(await _badgeRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = badges
        };
    }

    public async Task<ServiceActionResult> GetByIdAsync(Guid id)
    {
        var badge = _mapper.Map<DtoBadge>((await _badgeRepository
            .FindAsync(b => b.IsActive && b.Id == id)).FirstOrDefault() ?? throw new BadgeNotFoundException(id,nameof(Badge.Id)));
        return new ServiceActionResult(true)
        {
            Data = badge
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateBadgeRequest request)
    {
        var existedBadge = (await _badgeRepository
            .FindAsync(b => b.IsActive && b.Id == request.Id)).FirstOrDefault() ?? throw new BadgeNotFoundException(request.Id,nameof(Badge.Id));
        _mapper.Map(request, existedBadge);
        await _badgeRepository.UpdateAsync(existedBadge);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public Task<ServiceActionResult> DeactivateAsync(Guid buildingId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var badges = _mapper.ProjectTo<DtoBadge>(await _badgeRepository.FindAsync(x => x.IsActive));
        var paginatedResult = PaginationHelper.BuildPaginatedResult(badges, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }
}