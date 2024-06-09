using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
using Vigig.Domain.Enums;
using Vigig.Service.Constants;
using Vigig.Service.Enums;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Booking;

namespace Vigig.Service.Implementations;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IProviderServiceRepository _proServiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IBuildingRepository buildingRepository, IProviderServiceRepository proServiceRepository, IVigigUserRepository vigigUserRepository)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
        _buildingRepository = buildingRepository;
        _proServiceRepository = proServiceRepository;
        _vigigUserRepository = vigigUserRepository;
    }

    public async Task<ServiceActionResult> PlaceBookingAsync(string token, BookingPlaceRequest request)
    {
        var clientId = _jwtService.GetSubjectClaim(token)!.ToString();
        var client = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == clientId))
            .FirstOrDefault() ?? throw new UserNotFoundException(clientId,nameof(VigigUser.Id));
        
        var building = (await _buildingRepository.FindAsync(x => x.IsActive && x.Id == request.BuildingId))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(request.BuildingId,nameof(Building.Id));
        var providerService =
            (await _proServiceRepository.FindAsync(x => x.IsActive && x.Id == request.ProviderServiceId))
            .Include(x => x.Provider)
            .Include(x => x.Service)
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(request.ProviderServiceId,nameof(ProviderService.Id));

        var booking = new Booking
        {
            Apartment = request.Apartment,
            Building = building,
            Status = (int)BookingStatus.Pending,
            ProviderService = providerService,
            BookerName = (string.IsNullOrWhiteSpace(request.BookerName)) ? client.UserName : request.BookerName,
            BookerPhone = (string.IsNullOrWhiteSpace(request.BookerPhone)) ? client.Phone! : request.BookerPhone!,
            CreatedDate = DateTime.Now,
            VigigUser = client,
            StickerPrice = providerService.StickerPrice
        };
        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoPlacedBooking>(booking),
            StatusCode = StatusCodes.Status201Created
        }; 
    }

    public async Task<DtoPlacedBooking> RetrievedPlaceBookingAsync(string token, BookingPlaceRequest request)
    {
        var clientId = _jwtService.GetSubjectClaim(token)!.ToString();
        var client = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == clientId))
            .FirstOrDefault() ?? throw new UserNotFoundException(clientId,nameof(VigigUser.Id));
        
        var building = (await _buildingRepository.FindAsync(x => x.IsActive && x.Id == request.BuildingId))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(request.BuildingId,nameof(Building.Id));
        var providerService =
            (await _proServiceRepository.FindAsync(x => x.IsActive && x.Id == request.ProviderServiceId))
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(request.ProviderServiceId,nameof(ProviderService.Id));

        var booking = new Booking
        {
            Apartment = request.Apartment,
            Building = building,
            Status = (int)BookingStatus.Pending,
            ProviderService = providerService,
            BookerName = (string.IsNullOrWhiteSpace(request.BookerName)) ? client.UserName : request.BookerName,
            BookerPhone = (string.IsNullOrWhiteSpace(request.BookerPhone)) ? client.Phone! : request.BookerPhone!,
            CreatedDate = DateTime.Now,
            VigigUser = client,
            StickerPrice = providerService.StickerPrice
        };
        await _bookingRepository.AddAsync(booking);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<DtoPlacedBooking>(booking);
    }

    public async Task<ServiceActionResult> AcceptBookingAsync(Guid id, string token)
    {
        var isValidProvider = EnsureHasBookingAsync(token, id);
        if (!(await isValidProvider))
            throw new Exception($"provider do not have booking id:{id}");
        var booking = (await _bookingRepository.FindAsync(x => x.Id == id 
                                                               && x.Status == (int) BookingStatus.Pending
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id,nameof(Building.Id));
        booking.Status = BookingStatus.Accepted;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeclineBookingAsync(Guid id, string token)
    {
        var isValidProvider = EnsureHasBookingAsync(token, id);
        if (!(await isValidProvider))
            throw new Exception($"provider do not have booking id:{id}");
        var booking = (await _bookingRepository.FindAsync(x => x.Id == id 
                                                               && x.Status ==  BookingStatus.Pending
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id,nameof(Building.Id));

        booking.Status = BookingStatus.Declined;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> CancelBookingByClientAsync(Guid id, string token)
    {
        var isValidProvider = EnsureHasBookingAsync(token, id);
        if (!(await isValidProvider))
            throw new Exception($"provider do not have booking id:{id}");
        var booking = (await _bookingRepository.FindAsync(x => x.Id == id 
                                                               && x.Status ==  BookingStatus.Accepted 
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id,nameof(Building.Id));

        booking.Status = BookingStatus.CancelledByClient;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> CancelBookingByProviderAsync(Guid id, string token)
    {
        var isValidProvider = EnsureHasBookingAsync(token, id);
        if (!(await isValidProvider))
            throw new Exception($"provider do not have booking id:{id}");
        var booking = (await _bookingRepository.FindAsync(x => x.Id == id
                                                               && x.Status ==  BookingStatus.Accepted
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id,nameof(Building.Id));

        booking.Status = BookingStatus.CancelledByProvider;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> CompleteBookingAsync(Guid id, BookingCompleteRequest request, string token)
    {
        var isValidProvider = EnsureHasBookingAsync(token, id);
        if (!(await isValidProvider))
            throw new Exception($"provider do not have booking id:{id}");
        var booking = (await _bookingRepository.FindAsync(x => x.Id == id 
                                                               && x.Status == BookingStatus.Accepted
                                                               && x.IsActive))
            .Include( x => x.ProviderService)
            .FirstOrDefault() ?? throw new BookingNotFoundException(id,nameof(Building.Id));
        booking.Status = BookingStatus.Completed;
        booking.ProviderService.TotalBooking++;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> LoadOwnChatBookingAsync(string token)
    {
        var userId = _jwtService.GetSubjectClaim(token).ToString();
        var userRole = _jwtService.GetRoleClaim(token);
        var bookings = userRole switch
        {
            UserRoleConstant.Client => (await _bookingRepository.FindAsync(x =>
                                             x.IsActive && x.CustomerId.ToString() == userId && x.Status ==BookingStatus.Accepted))
                                         .Include(x => x.BookingMessages)
                                         ?? throw new UserNotFoundException(userId, nameof(VigigUser.Id)),
            UserRoleConstant.Provider => (await _bookingRepository.FindAsync(x =>
                                             x.IsActive && x.ProviderService.ProviderId.ToString() == userId && x.Status ==BookingStatus.Accepted))
                                         .Include(x => x.BookingMessages)
                                         ?? throw new UserNotFoundException(userId, nameof(VigigUser.Id)),
            _ => new List<Booking>().AsQueryable(),
        };

        return new ServiceActionResult(true)
        {
            Data = _mapper.ProjectTo<DtoBookChat>(bookings)
        };
    }

    public async Task<ServiceActionResult> RatingBookingAsync(string token, Guid bookingId, BookingRatingRequest request)
    {
        var user = _jwtService.GetAuthModel(token);
        var booking = await GetBookingForClientAsync(user.UserId, bookingId, user.UserName);
        booking.CustomerReview = request.Review;
        booking.CustomerRating = request.Rating;
        booking.ProviderService.Rating = 
            AverageHelper.GetAverage(booking.ProviderService.Rating, booking.ProviderService.RatingCount, request.Rating);
        booking.ProviderService.RatingCount++;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> LoadAllBookingsAsync(string token)
    {
        var user = _jwtService.GetAuthModel(token);
        var bookings = user.Role switch
        {
            UserRoleConstant.Client => (await _bookingRepository.FindAsync(
                    x => x.IsActive && x.CustomerId==user.UserId)) ?? throw new UserNotFoundException(user.UserId, nameof(VigigUser.Id)),
            UserRoleConstant.Provider => (await _bookingRepository.FindAsync(
                x => x.IsActive && x.ProviderService.ProviderId == user.UserId ))?? throw new UserNotFoundException(user.UserId, nameof(VigigUser.Id)),
            _ => new List<Booking>().AsQueryable(),
        };
        return new ServiceActionResult(true)
        {
            Data = _mapper.ProjectTo<DtoBooking>(bookings)
        };
    }

    private async Task<bool> EnsureHasBookingAsync(string token, Guid bookingId)
    {
        var providerId = _jwtService.GetSubjectClaim(token);
        var hasBooking = await _bookingRepository.ExistsAsync(x =>
            x.Id == bookingId && x.ProviderService.ProviderId == Guid.Parse(providerId.ToString()));
        if (hasBooking)
            return true;
        return false;
    }
    private async Task<Booking> GetBookingForClientAsync(Guid userId, Guid bookingId, string userName)
    {
        var booking = (await _bookingRepository.FindAsync(x =>
            x.IsActive &&
            x.CustomerId == userId &&
            x.Id == bookingId &&
            x.Status == BookingStatus.Completed))
            .Include(x => x.ProviderService).FirstOrDefault();

        if (booking == null)
        {
            throw new Exception($"{userName} does not have booking {bookingId}");
        }

        return booking;
    }
}