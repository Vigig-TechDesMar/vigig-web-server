using AutoMapper;
using Vigig.Common.Interfaces;
using Vigig.DAL.Data;
using Vigig.DAL.Implementations;
using Vigig.DAL.Interfaces;
using Vigig.Service.AutoMappings;

namespace Vigig.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        // services.AddScoped<>()
        services.AddScoped<IAppDbContext,VigigContext>();
        var registerableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAutoRegisterable).IsAssignableFrom(type) && type.IsInterface)
            .ToList();
        foreach (var type in registerableTypes)
        {
            var implementationType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            if (implementationType != null)
                services.AddScoped(type, implementationType);
        }

        var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}