using Vigig.Common.Constants;

namespace Vigig.Common.Settings;

public class CorSetting
{
    public string AllowedOrigins { get; init; }
    public string AllowedMethods { get; init; }
    public string AllowedHeaders { get; init; }
    public bool AllowCredentials { get; init; }

    public string[] GetAllowedOriginsArray()
    {
        return AllowedOrigins.Split(CorsConstant.HOSTS_SEPARATOR);
    }
    
    public string[] GetAllowedMethodsArray()
    {
        return AllowedMethods.Split(CorsConstant.METHODS_SEPARATOR);
    }
    
    public string[] GetAllowedHeadersArray()
    {
        return AllowedHeaders.Split(CorsConstant.HEADERS_SEPARATOR);
    }

    public bool AllowAnyOrigin()
    {
        return AllowedOrigins.Trim() == CorsConstant.ANY_ORIGIN;
    }
    
    public bool AllowAnyMethod()
    {
        return AllowedHeaders.Trim() == CorsConstant.ANY_METHOD;
    }
    
    public bool AllowAnyHeader()
    {
        return AllowedMethods.Trim() == CorsConstant.ANY_HEADER;
    }
}