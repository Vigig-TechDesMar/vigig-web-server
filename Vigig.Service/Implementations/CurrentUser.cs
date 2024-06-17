using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;

namespace Vigig.Service.Implementations;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
       
    }
    public AuthModel User { get; set; }
}