using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Implementations;

public class BookingFeeService : IBookingFeeService
{
    private readonly IBookingFeeRepository _bookingFeeRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingFeeService(IBookingFeeRepository bookingFeeRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _bookingFeeRepository = bookingFeeRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var bookingFees = _mapper.ProjectTo<DtoBookingFee>(await _bookingFeeRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = bookingFees
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var bookingFee = (await _bookingFeeRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault();
        if (bookingFee is null)
            throw new BookingFeeNotFoundException(id.ToString(),nameof(BookingFee.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBookingFee>(bookingFee)
        };

    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var bookingFees = _mapper.ProjectTo<DtoBookingFee>(
            await _bookingFeeRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoBookingFee>(bookingFees, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchBookingFee(SearchUsingGet request)
    {
        var bookingFees = (await _bookingFeeRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoBookingFee>>(SearchHelper.BuildSearchResult<BookingFee>(bookingFees, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateBookingFeeRequest request)
    {
        if (!await _bookingRepository.ExistsAsync(sc=> sc.Id == request.BookingId))
            throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));
        var bookingFee = _mapper.Map<BookingFee>(request);
        await _bookingFeeRepository.AddAsync(bookingFee);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBookingFee>(bookingFee),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateBookingFeeRequest request)
    {
        if (!await _bookingRepository.ExistsAsync(sc=> sc.Id == request.BookingId))
            throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));
        var bookingFee = (await _bookingFeeRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault() ??
                      throw new BookingFeeNotFoundException(request.Id,nameof(BookingFee.Id));

        _mapper.Map(request, bookingFee);
        await _bookingFeeRepository.UpdateAsync(bookingFee);
        await _unitOfWork.CommitAsync();

        _mapper.Map<BookingFee, DtoBookingFee>(bookingFee);
        return new ServiceActionResult(true)
        {
            Data = bookingFee,
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}