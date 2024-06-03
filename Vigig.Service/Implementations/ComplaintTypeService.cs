using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Complaint;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Complaint;

namespace Vigig.Service.Implementations;

public class ComplaintTypeService : IComplaintTypeService
{
    private readonly IComplaintTypeRepository _complaintTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ComplaintTypeService(IComplaintTypeRepository complaintTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _complaintTypeRepository = complaintTypeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var complaintTypes = _mapper.ProjectTo<DtoComplaintType>(await _complaintTypeRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = complaintTypes
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var complaintType =
            (await _complaintTypeRepository.FindAsync(sc => sc.Id == id && sc.IsActive)).FirstOrDefault();
        if (complaintType is null)
            throw new ComplaintTypeNotFoundException(id.ToString(),nameof(ComplaintType.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaintType>(complaintType)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var complaintTypes = _mapper.ProjectTo<DtoComplaintType>(
            await _complaintTypeRepository.FindAsync(s => s.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoComplaintType>(complaintTypes, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchComplaintType(SearchUsingGet request)
    {
        var complaintTypes = (await _complaintTypeRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoComplaintType>>(
                SearchHelper.BuildSearchResult<ComplaintType>(complaintTypes, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }
    
    //Admin
    public async Task<ServiceActionResult> AddAsync(CreateComplaintTypeRequest request)
    {
        var complaintType = _mapper.Map<ComplaintType>(request);
        await _complaintTypeRepository.AddAsync(complaintType);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaintType>(complaintType),
            StatusCode = StatusCodes.Status201Created
        };
    }

    //Admin
    public async Task<ServiceActionResult> UpdateAsync(UpdateComplaintTypeRequest request)
    {
        var complaintType = (await _complaintTypeRepository.FindAsync(sc => sc.Id == request.Id && sc.IsActive))
                            .FirstOrDefault()
                            ?? throw new ComplaintTypeNotFoundException(request.Id,nameof(ComplaintType.Id));
        _mapper.Map(request, complaintType);
        await _complaintTypeRepository.UpdateAsync(complaintType);
        await _unitOfWork.CommitAsync();

        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaintType>(complaintType),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    //Admin
    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}