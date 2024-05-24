using System.Collections.Concurrent;

namespace Vigig.Api.Hubs.Models;

public class BookingConnectionPool
{
    private readonly ConcurrentDictionary<string, List<string>> _connectionPool = new ConcurrentDictionary<string, List<string>>();
    public ConcurrentDictionary<string, List<string>> connectionPool => _connectionPool;
}