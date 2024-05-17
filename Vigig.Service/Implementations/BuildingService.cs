using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Building;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Building;

namespace Vigig.Service.Implementations;

public class BuildingService : IBuildingService
{
    private readonly IBuildingRepository _buildingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BuildingService(IBuildingRepository buildingRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _buildingRepository = buildingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> AddAsync(CreateBuildingRequest request)
    {
        var existedBuilding =
            (await _buildingRepository.FindAsync(b => b.BuildingName.Equals(request.BuildingName) && b.IsActive))
            .FirstOrDefault();
        if (existedBuilding is not null)
            throw new BuildingAlreadyExistException(request.BuildingName);
        var building = _mapper.Map<Building>(request);
        await _buildingRepository.AddAsync(building);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBuilding>(building),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        return new ServiceActionResult(true)
        {
            Data = _mapper.ProjectTo<DtoBuilding>(await _buildingRepository.GetAllAsync())
        };
    }

    public async Task<ServiceActionResult> GetById(Guid buildingId)
    {
        var building = (await _buildingRepository.FindAsync(b => b.Id == buildingId && b.IsActive)).FirstOrDefault()
                       ?? throw new BuildingNotFoundException(buildingId.ToString());
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBuilding>(building)
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateBuildingRequest building)
    {
        var existedBuilding = (await _buildingRepository.FindAsync(b => b.Id == building.Id && b.IsActive)).FirstOrDefault()
            ?? throw new BuildingNotFoundException(building.Id.ToString());
        existedBuilding.BuildingName = (string.IsNullOrEmpty(building.BuildingName)) ? existedBuilding.BuildingName : building.BuildingName;
        existedBuilding.Note = (string.IsNullOrEmpty(building.Note)) ? existedBuilding.Note : building.Note;
        
        await _buildingRepository.UpdateAsync(existedBuilding);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeactivateAsync(Guid buildingId)
    {
        var building = (await _buildingRepository.FindAsync(b => b.IsActive && b.Id == buildingId)).FirstOrDefault()
                       ?? throw new BuildingNotFoundException(buildingId.ToString());
        building.IsActive = false;
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var building = _mapper.ProjectTo<DtoBuilding>(await _buildingRepository.GetAllAsync());
        var paginatedResult = PaginationHelper.BuildPaginatedResult(building, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }
}