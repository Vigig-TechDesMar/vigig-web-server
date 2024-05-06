using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using NLog;
using Vigig.Common.Constants;
using Vigig.Common.Constants.Messages;
using Vigig.Common.Exceptions;

namespace Vigig.Common.Helpers;

public class DataAccessHelper
{
    private static IConfiguration _configuration;
    private static readonly ILogger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);

    private static IConfiguration Configuration
    {
        get => _configuration ?? throw new Exception("Configuration is not initialized");
        set => _configuration = value;
    }

    public static void InitConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string GetConnectionString(string connectionName) =>
        Configuration.GetConnectionString(connectionName) ?? throw new MissingConnectionStringException();

    public static string GetDefaultConnectionString() => GetConnectionString(DataAccessConstant.DefaultConnectionName);

    public static void EnsureMigrations(string assemblyName, string? connection = null)
    {
        connection ??= GetDefaultConnectionString();
        EnsureDatabase.For.SqlDatabase(connection);
        var upgradeEngine = DeployChanges.To.SqlDatabase(connection)
            .WithScriptsEmbeddedInAssembly(Assembly.Load(assemblyName))
            .LogToConsole()
            .Build();
        var scripts = upgradeEngine.GetDiscoveredScripts();
        if (scripts.Any())
        {
            upgradeEngine.PerformUpgrade(); }
        else
        {
            logger.Info(InformationMessage.NoNewScriptFound);
        }
    }

}