using Microsoft.Extensions.DependencyInjection;

namespace Vigig.Common.Attribute;

public class ServiceRegisterAttribute : System.Attribute
{
    private ServiceLifetime LifeTime { get; set; }

    public ServiceRegisterAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        LifeTime = lifetime;
    }
}