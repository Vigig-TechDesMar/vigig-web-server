using System.Collections.Concurrent;

namespace Vigig.Api.Hubs.Models;

public class ChatConnectionPool
{
    private readonly ConcurrentDictionary<string, List<string>> _connections =
        new ConcurrentDictionary<string, List<string>>();


    public ConcurrentDictionary<string, List<string>> connections => _connections;
}