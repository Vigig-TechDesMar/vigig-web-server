using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Vigig.Common.Constants;
using Vigig.Common.Exceptions;
using Vigig.Common.Extensions;
using Vigig.Common.Helpers;
using Vigig.Common.Settings;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.Implementations;

public class GoogleOauthService : IGoogleOauthService
{
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IVigigRoleRepository _vigigRoleRepository;
    private readonly GoogleSetting _googleSetting;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public GoogleOauthService(IVigigUserRepository vigigUserRepository, IConfiguration configuration, IEmailService emailService, IUnitOfWork unitOfWork, IJwtService jwtService, IMapper mapper, IVigigRoleRepository vigigRoleRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _configuration = configuration;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _mapper = mapper;
        _vigigRoleRepository = vigigRoleRepository;
        _googleSetting = _configuration.GetSection(nameof(GoogleSetting)).Get<GoogleSetting>() ??
                         throw new MissingGoogleConfiguration();
    }

    public async Task<ServiceActionResult> LoginAsync(OAuthRequest request)
    {
        var googleLoginRequest = (GoogleAuthRequest)request;
        var client = new HttpClient();
        var code = WebUtility.UrlDecode(googleLoginRequest.Code);

        var requestParams = new Dictionary<string, string>()
        {
            { GoogleOauthConstant.CODE, code },
            { GoogleOauthConstant.CLIENT_ID, _googleSetting.ClientId },
            { GoogleOauthConstant.CLIENT_SECRET, _googleSetting.ClientSecret },
            { GoogleOauthConstant.REDIRECT_URI, _googleSetting.RedirectUri },
            { GoogleOauthConstant.GRANT_TYPE, GoogleOauthConstant.AUTHORIZATION_CODE }
        };

        var content = new FormUrlEncodedContent(requestParams);
        var response = await client.PostAsync(GoogleOauthConstant.GOOGLE_TOKEN_URL, content);
        if (!response.IsSuccessStatusCode)
            throw new InvalidTokenException();
        var authObject = JsonConvert.DeserializeObject<GoogleAuthResponse>(await response.Content.ReadAsStringAsync());

        if (authObject?.IdToken == null)
            throw new Exception("Can not get id token from google");

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(authObject.IdToken);
        securityToken.Claims.TryGetValue(GoogleTokenClaimConstants.EMAIL, out var email);
        securityToken.Claims.TryGetValue(GoogleTokenClaimConstants.EMAIL_VERIFIED, out var emailVerified);
        securityToken.Claims.TryGetValue(GoogleTokenClaimConstants.GIVEN_NAME, out var name);
        securityToken.Claims.TryGetValue(GoogleTokenClaimConstants.PICTURE, out var picture);

        var user = (await _vigigUserRepository.FindAsync(x => x.Email == email)).Include(x => x.Roles).FirstOrDefault() ??
                   await CreateNewUserAsync(email, emailVerified, name, picture);
        var roles = user.Roles.Select(x => x.Name).ToList();
        var authResponse = new AuthResponse
        {
            Name = user.UserName ?? user.Email ?? string.Empty,
            Roles = roles,
            Token = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user, roles),
                RefreshToken = await _jwtService.GenerateRefreshToken(user.Id),
                ExpiresAt = DateTimeOffset.Now.AddHours(1)
            }
        };
        return new ServiceActionResult()
        {
            Data = new{
                UserInfo = new{
                    Name = user.UserName,
                    Email = user.Email
                },
                token = authResponse 
            }
        };
    }
    private async Task<VigigUser> CreateNewUserAsync(string email, string emailVerified, string fullName, string picture)
    {
        var user = new VigigUser
        {
            Email = email,
            EmailConfirmed = emailVerified.ToBool(),
            UserName = email,
            FullName = fullName,
            ProfileImage = picture,
            CreatedDate = DateTime.Now,
            NormalizedEmail = email.Normalize(),
            NormalizedUserName = email.Normalize()
        };
        var autoGeneratedPassword = RandomPasswordHelper.GenerateRandomPassword(10);
        var hashedPassword = PasswordHashHelper.HashPassword(autoGeneratedPassword);
        user.Password = hashedPassword;
        await _vigigUserRepository.AddAsync(user);
        var role = (await _vigigRoleRepository.FindAsync(x => x.NormalizedName == UserRoleConstant.Client)).FirstOrDefault() ?? throw new RoleNotFoundException(UserRoleConstant.Client);    
        user.Roles.Add(role);
        await _unitOfWork.CommitAsync();


       
        var userReturned = (await _vigigUserRepository.FindAsync(x => x.Email == email)).FirstOrDefault();
        if (userReturned == null)
            throw new Exception("Can not find user by email");
        return userReturned;
    }
}