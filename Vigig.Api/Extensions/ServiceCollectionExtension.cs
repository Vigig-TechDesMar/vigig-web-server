using AutoMapper;
using Vigig.DAL.Data;
using Vigig.DAL.Implementations;
using Vigig.DAL.Interfaces;

namespace Vigig.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        // services.AddScoped<>()
        services.AddScoped<IAppDbContext,VigigContext>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}