using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Authentication;

namespace Vigig.Service.Interfaces;
public interface IOauthService
{
    Task<ServiceActionResult> LoginAsync(OAuthRequest request);
}