using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Domain.Enums;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Implementations;

public class BookingFeeService : IBookingFeeService
{
    private readonly IBookingFeeRepository _bookingFeeRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IJwtService _jwtService;
    private readonly ITransactionService _transactionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingFeeService(IBookingFeeRepository bookingFeeRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IMapper mapper, IVigigUserRepository vigigUserRepository, IJwtService jwtService, ITransactionService transactionService, IGigServiceRepository gigServiceRepository)
    {
        _bookingFeeRepository = bookingFeeRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _vigigUserRepository = vigigUserRepository;
        _jwtService = jwtService;
        _transactionService = transactionService;
        _gigServiceRepository = gigServiceRepository;
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

    public async Task<ServiceActionResult> AddAsyncFromBooking(Booking booking, string token)
    {
        //Validate provider
        var userId = _jwtService.GetSubjectClaim(token).ToString();
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == userId))
            .Include(x => x.Wallets)
            .FirstOrDefault() ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));

        if (_jwtService.GetAuthModel(token).Role == UserRoleConstant.Client)
            throw new UnauthorizedAccessException("Customers are not allowed!");

        //Get the wallet
        var wallet = provider.Wallets.FirstOrDefault() ?? throw new WalletNotFoundException(userId, nameof(userId));

        var book = (await _bookingRepository.FindAsync(x=> x.Id == booking.Id)).Include(x=> x.ProviderService).ThenInclude(x=> x.Service).FirstOrDefault();
        var feePercentage = book?.ProviderService.Service.Fee??0;
        var stickerPrice = book?.ProviderService.StickerPrice??0;
        
        var bookingFee = new BookingFee
        {
            Amount = Math.Round(stickerPrice*feePercentage/1000),
            CreatedDate = DateTime.Now,
            Status = CashStatus.Pending,
            BookingId = booking.Id,
            Booking = booking
        };

        await _bookingFeeRepository.AddAsync(bookingFee);
        
        // Process the transaction
        await _transactionService.ProcessTransactionAsync(bookingFee,wallet);
        
        await _unitOfWork.CommitAsync();
        
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBookingFee>(bookingFee),
            StatusCode = StatusCodes.Status201Created
        };
    }
    //Admin
    public async Task<ServiceActionResult> AddAsync(CreateBookingFeeRequest request, string token)
    {
        if (_jwtService.GetAuthModel(token).Role != UserRoleConstant.InternalUser)
            throw new UnauthorizedAccessException("Users are not allowed!");
        
        if (!await _bookingRepository.ExistsAsync(sc=> sc.Id == request.BookingId))
            throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));
        
        var bookingFee = _mapper.Map<BookingFee>(request);
        bookingFee.CreatedDate = DateTime.Now;
        
        await _bookingFeeRepository.AddAsync(bookingFee);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoBookingFee>(bookingFee),
            StatusCode = StatusCodes.Status201Created
        };
    }

    // public async Task<ServiceActionResult> UpdateAsync(UpdateBookingFeeRequest request)
    // {
    //     if (!await _bookingRepository.ExistsAsync(sc=> sc.Id == request.BookingId))
    //         throw new BookingNotFoundException(request.BookingId,nameof(Booking.Id));
    //     var bookingFee = (await _bookingFeeRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault() ??
    //                   throw new BookingFeeNotFoundException(request.Id,nameof(BookingFee.Id));
    //
    //     _mapper.Map(request, bookingFee);
    //     await _bookingFeeRepository.UpdateAsync(bookingFee);
    //     await _unitOfWork.CommitAsync();
    //
    //     _mapper.Map<BookingFee, DtoBookingFee>(bookingFee);
    //     return new ServiceActionResult(true)
    //     {
    //         Data = bookingFee,
    //         StatusCode = StatusCodes.Status204NoContent
    //     };
    // }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}