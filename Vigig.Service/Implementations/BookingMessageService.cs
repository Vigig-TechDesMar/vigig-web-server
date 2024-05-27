using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;

namespace Vigig.Service.Implementations;

public class BookingMessageService : IBookingMessageService
{
    private readonly IBookingMessageRepository _messageRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingMessageService(IBookingMessageRepository messageRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper, IBookingRepository bookingRepository, IJwtService jwtService)
    {
        _messageRepository = messageRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _bookingRepository = bookingRepository;
        _jwtService = jwtService;
    }



    public async Task<BookingMessage> SendMessage(string token, Guid bookingId, string message)
    {
        var senderId = _jwtService.GetSubjectClaim(token);
        
        var sender = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == senderId))
            .Include(x => x.Bookings)
            .FirstOrDefault() ?? throw new UserNotFoundException(senderId,nameof(VigigUser.Id));
        if (!sender.Bookings.Any(booking => booking.Id == bookingId))
            throw new Exception($"{sender.UserName} does not have booking id: {bookingId}");
        var booking = await _bookingRepository.GetAsync(x => x.IsActive && x.Id == bookingId);
        
        var bookingMessage = new BookingMessage
        {
            SenderName = sender.UserName,
            Content = message.Trim(),
            Booking = booking,
            SentAt = DateTime.Now
        };
        await _messageRepository.AddAsync(bookingMessage);
        await _unitOfWork.CommitAsync();

        return bookingMessage;
    }

    public async Task<IQueryable> LoadAllBookingMessage(string token, Guid bookingId)
    {
        var isValid = await EnsureHasBookingAsync(token, bookingId);
        if (!isValid)
            throw new Exception($"User does not have booking id: {bookingId}");
        var messages = await _messageRepository.FindAsync(x => x.BookingId == bookingId);
        return messages;
    }

    private async Task<bool> EnsureHasBookingAsync(string token, Guid bookingId)
    {
        var providerId = _jwtService.GetSubjectClaim(token);
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == providerId))
            .Include(x => x.Bookings)
            .FirstOrDefault() ?? throw new UserNotFoundException(providerId,nameof(VigigUser.Id));
        return false;
    }
}