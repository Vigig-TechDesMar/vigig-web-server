using System.Reflection;
using AutoMapper;
using Vigig.Common.Attribute;
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

        var registeredServiceAttribute = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsInterface && type.GetCustomAttributes<ServiceRegisterAttribute>().Any())
            .ToList();

        foreach (var type in registeredServiceAttribute)
        {
            var attribute = type.GetCustomAttributes<ServiceRegisterAttribute>().FirstOrDefault()
                ?? throw new Exception("Not found ServiceRegisterAttribute");
            
            var lifeTimeProperty =  attribute.GetType().GetProperty("LifeTime", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new Exception("Not found life time") ;
            
            var lifeTime = lifeTimeProperty.GetValue(attribute);
            if (lifeTime is ServiceLifetime l)
            {
                var implementType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                
                if (implementType is not null)
                    services.Add(new ServiceDescriptor(type,implementType,l ));
            }
        }
        // var registerableTypes = AppDomain.CurrentDomain.GetAssemblies()
        //     .SelectMany(assembly => assembly.GetTypes())
        //     .Where(type => typeof(IAutoRegisterable).IsAssignableFrom(type) && type.IsInterface)
        //     .ToList();
        // foreach (var type in registerableTypes)
        // {
        //     var implementationType = AppDomain.CurrentDomain.GetAssemblies()
        //         .SelectMany(assembly => assembly.GetTypes())
        //         .FirstOrDefault(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        //     if (implementationType != null)
        //         services.AddScoped(type, implementationType);
        // }

        var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
}