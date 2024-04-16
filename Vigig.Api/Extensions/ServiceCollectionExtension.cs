using AutoMapper;

namespace Vigig.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        // services.AddScoped<>()
        return services;
    }
}