using Microsoft.AspNetCore.SignalR;
using Vigig.Api.Hubs.Models;
using Vigig.Domain.Dtos.Booking;
using Vigig.Domain.Entities;
using Vigig.Service.Interfaces;

namespace Vigig.Api.Hubs;

public class ChatHub : Hub
{
    private readonly ChatConnectionPool _chatConnectionPool;
    private readonly IBookingMessageService _bookingMessageService;
    private IJwtService _jwtService;

    public ChatHub(ChatConnectionPool chatConnectionPool, IBookingMessageService bookingMessageService, IJwtService jwtService)
    {
        _chatConnectionPool = chatConnectionPool;
        _bookingMessageService = bookingMessageService;
        _jwtService = jwtService;
    }

    public override Task<string> OnConnectedAsync()
    {
        var accessToken = Context.GetHttpContext().Request.Query["access_token"].ToString();
        var userId = _jwtService.GetSubjectClaim(accessToken).ToString();
        var connectionId = Context.ConnectionId;

        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(connectionId))
        {
            if (!_chatConnectionPool.connections.ContainsKey(userId))
            {
                _chatConnectionPool.connections[userId] = new ();
            }
            _chatConnectionPool.connections[userId].Add(string.Empty);
        }
        base.OnConnectedAsync();
        return Task.FromResult("aaa");
    }

    public async Task<IEnumerable<DtoBookingMessage>>JoinSpecificChatRoom(string bookingId)
    {
        
        await Groups.AddToGroupAsync(Context.ConnectionId, bookingId);
        var token = Context.GetHttpContext()!.Request.Query["access_token"].ToString();
        var userId = _jwtService.GetSubjectClaim(token).ToString();
        _chatConnectionPool.connections[userId].Add(bookingId);
        var messages = await _bookingMessageService.LoadAllBookingMessage(token,Guid.Parse(bookingId));
        // await Clients.Group(bookingId)
        //     .SendAsync("LoadAllMessage",messages);

        return messages.ToList();
    }

    public async Task SendMessage(string msg, string bookingId)
    {
        var token = Context.GetHttpContext()!.Request.Query["access_token"].ToString();
        var message = await _bookingMessageService.SendMessage(token, Guid.Parse(bookingId), msg);
        // Console.WriteLine(Clients.Groups()); 
        await Clients.Group(bookingId)
            .SendAsync("ReceiveSpecificMessage", message);
    }

    public async Task IsChatting(string bookingId)
    {
        await Clients.OthersInGroup(bookingId)
            .SendAsync("otherChatting");
    }

    public async Task StopChatting(string bookingId)
    {
        await Clients.OthersInGroup(bookingId)
            .SendAsync("otherStopChatting"); 
    }
}