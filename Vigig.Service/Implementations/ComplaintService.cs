using AutoMapper;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Tls;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Complaint;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Complaint;

namespace Vigig.Service.Implementations;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IComplaintTypeRepository _complaintTypeRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ComplaintService(IComplaintRepository complaintRepository, IComplaintTypeRepository complaintTypeRepository, IUnitOfWork unitOfWork, IMapper mapper, IBookingRepository bookingRepository)
    {
        _complaintRepository = complaintRepository;
        _complaintTypeRepository = complaintTypeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _bookingRepository = bookingRepository;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var complaints = _mapper.ProjectTo<DtoComplaint>(await _complaintRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = complaints
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var complaint = (await _complaintRepository.FindAsync(sc => sc.Id == id && sc.IsActive)).FirstOrDefault()
                        ?? throw new ComplaintNotFoundException(id.ToString(),nameof(Complaint.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaint>(complaint)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var complaints = _mapper.ProjectTo<DtoComplaint>(
            await _complaintRepository.FindAsync(s => s.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoComplaint>(complaints, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchComplaint(SearchUsingGet request)
    {
        var complaints = (await _complaintRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoComplaint>>(SearchHelper.BuildSearchResult<Complaint>(complaints, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateComplaintRequest request)
    {
        //Check Complaint Type
        if (!await _complaintTypeRepository.ExistsAsync(sc => sc.IsActive && sc.Id == request.ComplaintTypeId))
            throw new ComplaintTypeNotFoundException(request.ComplaintTypeId,nameof(ComplaintType.Id));
        
        //Check Booking
        if(request.BookingId != null)
            if (!await _bookingRepository.ExistsAsync(sc => sc.Id == request.BookingId))
                throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));

        var complaint = _mapper.Map<Complaint>(request);
        await _complaintRepository.AddAsync(complaint);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaint>(complaint),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateComplaintRequest request)
    {
        //Check Complaint Type
        if (!await _complaintTypeRepository.ExistsAsync(sc => sc.IsActive && sc.Id == request.ComplaintTypeId))
            throw new ComplaintTypeNotFoundException(request.ComplaintTypeId,nameof(ComplaintType.Id));
        
        //Check Booking
        if(request.BookingId != null)
            if (!await _bookingRepository.ExistsAsync(sc => sc.Id == request.BookingId))
                throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));
        
        //Check Complaint
        var complaint =
            (await _complaintRepository.FindAsync(sc => sc.Id == request.Id && sc.IsActive)).FirstOrDefault()
            ?? throw new ComplaintNotFoundException(request.Id,nameof(Complaint.Id));
        
        _mapper.Map(request, complaint);
        await _complaintRepository.AddAsync(complaint);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoComplaint>(complaint),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}