using Microsoft.AspNetCore.SignalR;
using Vigig.Api.Hubs.Models;
using Vigig.Service.Interfaces;

namespace Vigig.Api.Hubs;

public class ChatHub : Hub
{
    private readonly ChatConnectionPool _chatConnectionPool;
    private readonly IBookingMessageService _bookingMessageService;

    public ChatHub(ChatConnectionPool chatConnectionPool, IBookingMessageService bookingMessageService)
    {
        _chatConnectionPool = chatConnectionPool;
        _bookingMessageService = bookingMessageService;
    }

    public async Task JoinSpecificChatRoom(UserConnection conn)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conn.BookingId);
        _chatConnectionPool.connections[Context.ConnectionId] = conn;
        var token = Context.GetHttpContext()!.Request.Query["access_token"].ToString();
        var messages = await _bookingMessageService.LoadAllBookingMessage(token,Guid.Parse(conn.BookingId));
        await Clients.Group(conn.BookingId)
            .SendAsync("JoinSpecificChatRoom",messages);
    }

    public async Task SendMessage(string msg)
    {
        var token = Context.GetHttpContext()!.Request.Query["access_token"].ToString();

        if (_chatConnectionPool.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            await _bookingMessageService.SendMessage(token, Guid.Parse(conn.BookingId), msg);
            
            await Clients.Group(conn.BookingId)
                .SendAsync("ReceiveSpecificMessage", conn.UserName, msg);
        }   
    }
    
}