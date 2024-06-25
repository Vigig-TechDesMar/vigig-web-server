using Microsoft.AspNetCore.Mvc;
using Vigig.Common.Attribute;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IUserService
{
    Task<ServiceActionResult> GetProfileInformation(string token);
    Task<ServiceActionResult> UploadService(string token,CreateProviderServiceRequest request);
    Task<ServiceActionResult> UpdateProfile(string token, UpdateProfileRequest request);
}