using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Vigig.Common.Constants.Validations;
using Vigig.Common.Exceptions;
using Vigig.Common.Helpers;
using Vigig.Common.Settings;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;
using Vigig.Service.Enums;
using Vigig.Service.Exceptions;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IVigigRoleRepository _vigigRoleRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSetting _jwtSetting;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(IVigigUserRepository vigigUserRepository, IMapper mapper, IUnitOfWork unitOfWork, IBuildingRepository buildingRepository, IJwtService jwtService, IConfiguration configuration, IUserTokenRepository userTokenRepository, IVigigRoleRepository vigigRoleRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _buildingRepository = buildingRepository;
        _jwtService = jwtService;
        _userTokenRepository = userTokenRepository;
        _vigigRoleRepository = vigigRoleRepository;
        _jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() ?? throw new MissingJwtSettingsException();
    }
    public async Task<ServiceActionResult> RegisterAsync(RegisterRequest request)
    {
        var retrivedUser = await _vigigUserRepository.GetAsync(user => user.Email!.ToLower() == request.Email.ToLower());

        var role = (await _vigigRoleRepository.FindAsync(r => r.NormalizedName == request.Role.ToString()))
            .FirstOrDefault() ?? throw new RoleNotFoundException(request.Role.ToString());
        
        if (retrivedUser is not null)
            throw new UserAlreadyExistException(request.Email);

        if (!Regex.IsMatch(request.Email, UserProfileValidation.Email.EmailPattern))
            throw new EmailNotMatchedException();
        
        if (!Regex.IsMatch(request.Password, UserProfileValidation.Password.PasswordPattern))
            throw new PasswordTooWeakException();
        
        
        if (retrivedUser is null)
        {
            
            retrivedUser = _mapper.Map<VigigUser>(request);
            var hashedPassword = PasswordHashHelper.HashPassword(request.Password);
            retrivedUser.Password = hashedPassword;
            retrivedUser.CreatedDate = DateTime.Now;
            retrivedUser.NormalizedEmail = request.Email.ToUpper();
            retrivedUser.UserName = request.Email.Split("@")[0];
            retrivedUser.NormalizedUserName = retrivedUser.UserName.Split("@")[0].ToUpper();
            retrivedUser.Building =  (await _buildingRepository.FindAsync(b => b.Id == new Guid("e9f94484-6bf6-43eb-4827-08dc73e1398f"))).FirstOrDefault() 
                                     ?? throw new BuildingNotFoundException("e9f94484-6bf6-43eb-4827-08dc73e1398f");
            retrivedUser.Roles.Add(role);
            await _vigigUserRepository.AddAsync(retrivedUser);
        }
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true) { Data = _mapper.Map<RegisterResponse>(retrivedUser)};
    }

    public async Task<ServiceActionResult> LoginAsync(LoginRequest request)
    {
        var retrivedUser = await _vigigUserRepository.GetAsync(c => c.Email.Equals(request.Email));

        if (retrivedUser is null)
            throw new UserNotFoundException(request.Email);
        var isValidPassword = PasswordHashHelper.VerifyPassword(request.Password, retrivedUser.Password);
        if (!isValidPassword)
            throw new InvalidPasswordException();
        var authResponse = await GenerateAuthResponseAsync(retrivedUser);
        return new ServiceActionResult(true)
        {
            Data = new{
                UserInfo = new{
                    Name = retrivedUser.UserName,
                    Email = retrivedUser.Email
                },
                token = authResponse 
            }
        };
    }

    public async Task<ServiceActionResult> RefreshTokenAsync(RefreshTokenRequest token)
    {
        var refreshToken = await _userTokenRepository.GetAsync(t => t.Value == token.RefreshToken);
        if (refreshToken is null)
            throw new RefreshTokenNotFoundException(token.RefreshToken);
        var customer = await _vigigUserRepository.GetAsync(c => c.Id == refreshToken.UserId);
        if (customer is null)
            throw new UserNotFoundException(refreshToken.UserId.ToString());
        var tokenResponse = GenerateAuthResponseAsync(customer);
        return new ServiceActionResult()
        {
            Data = tokenResponse
        };
    }

    public async Task<AuthResponse> GenerateAuthResponseAsync(VigigUser vigigUser)
    {
        var reponse = new AuthResponse()
        {
            Name = vigigUser.UserName ?? vigigUser.Email ?? String.Empty,
            Role = vigigUser.Roles,
            Token = new TokenResponse()
            {
                AccessToken = _jwtService.GenerateAccessToken(vigigUser,vigigUser.Roles),
                RefreshToken = await _jwtService.GenerateRefreshToken(vigigUser.Id),
                ExpiresAt = DateTimeOffset.Now.AddHours(_jwtSetting.RefreshTokenLifetimeInMinutes)
            }
        };
        return reponse;
    }
}