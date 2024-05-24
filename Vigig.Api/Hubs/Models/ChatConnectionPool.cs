using System.Collections.Concurrent;

namespace Vigig.Api.Hubs.Models;

public class ChatConnectionPool
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections =
        new ConcurrentDictionary<string, UserConnection>();


    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}