using Vigig.Common.Attribute;

namespace Vigig.Service.BackgroundJobs.Interfaces;
[ServiceRegister]
public interface IExpirationService
{
    Task ValidateEventExpiration();
}