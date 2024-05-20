using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IUserService
{
    Task<ServiceActionResult> GetProfileInformation(string token);
    Task<ServiceActionResult> UploadService(CreateProviderServiceRequest request);
}