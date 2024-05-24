using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
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
            .FirstOrDefault() ?? throw new UserNotFoundException(clientId);
        
        var building = (await _buildingRepository.FindAsync(x => x.IsActive && x.Id == request.BuildingId))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(request.BuildingId);
        var providerService =
            (await _proServiceRepository.FindAsync(x => x.IsActive && x.Id == request.ProviderServiceId))
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(request.ProviderServiceId);

        var booking = new Booking
        {
            Apartment = "20.01",
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
            .FirstOrDefault() ?? throw new UserNotFoundException(clientId);
        
        var building = (await _buildingRepository.FindAsync(x => x.IsActive && x.Id == request.BuildingId))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(request.BuildingId);
        var providerService =
            (await _proServiceRepository.FindAsync(x => x.IsActive && x.Id == request.ProviderServiceId))
            .FirstOrDefault() ?? throw new ProviderServiceNotFoundException(request.ProviderServiceId);

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
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id);
        booking.Status = (int)BookingStatus.Accepted;
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
                                                               && x.Status == (int) BookingStatus.Pending
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id);

        booking.Status = (int)BookingStatus.Declined;
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
                                                               && x.Status == (int) BookingStatus.Accepted 
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id);

        booking.Status = (int)BookingStatus.CancelledByClient;
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
                                                               && x.Status == (int) BookingStatus.Accepted
                                                               && x.IsActive))
            .FirstOrDefault() ?? throw new BuildingNotFoundException(id);

        booking.Status = (int)BookingStatus.CancelledByProvider;
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
                                                               && x.Status == (int)BookingStatus.Accepted
                                                               && x.IsActive))
            .Include( x => x.ProviderService)
            .FirstOrDefault() ?? throw new BookingNotFoundException(id);
        booking.Status = (int)BookingStatus.Completed;
        booking.CustomerRating = request.CustomerRating;
        booking.CustomerReview = request.CustomerReview;
        booking.ProviderService.TotalBooking++;
        await _bookingRepository.UpdateAsync(booking);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    private async Task<bool> EnsureHasBookingAsync(string token, Guid bookingId)
    {
        var providerId = _jwtService.GetSubjectClaim(token);
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == providerId))
            .Include(x => x.Bookings)
            .FirstOrDefault() ?? throw new UserNotFoundException(providerId);
        foreach (var booking in provider.Bookings)
        {
            if (booking.Id == bookingId)
                return true;
        }
        return false;
    }
}