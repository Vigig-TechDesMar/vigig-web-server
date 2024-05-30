using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vigig.Common.Constants.Validations;
using Vigig.Common.Exceptions;
using Vigig.Common.Helpers;
using Vigig.Common.Settings;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IVigigRoleRepository _vigigRoleRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IBadgeRepository _badgeRepository;
    private readonly IJwtService _jwtService;
    private readonly JwtSetting _jwtSetting;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuthService(IVigigUserRepository vigigUserRepository, IMapper mapper, IUnitOfWork unitOfWork, IBuildingRepository buildingRepository, IJwtService jwtService, IConfiguration configuration, IUserTokenRepository userTokenRepository, IVigigRoleRepository vigigRoleRepository, IBadgeRepository badgeRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _buildingRepository = buildingRepository;
        _jwtService = jwtService;
        _userTokenRepository = userTokenRepository;
        _vigigRoleRepository = vigigRoleRepository;
        _jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() ?? throw new MissingJwtSettingsException();
        _badgeRepository = badgeRepository;
    }
    public async Task<ServiceActionResult> RegisterAsync(RegisterRequest request)
    {
        var retrivedUser = await _vigigUserRepository.GetAsync(user => user.Email!.ToLower() == request.Email.ToLower());

        var role = (await _vigigRoleRepository.FindAsync(r => r.NormalizedName == request.Role.ToString()))
            .FirstOrDefault() ?? throw new RoleNotFoundException(request.Role.ToString());
        
        var building =  (await _buildingRepository.FindAsync(b => b.Id == request.BuildingId)).FirstOrDefault() 
                        ?? throw new BuildingNotFoundException(request.BuildingId);
        
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
            retrivedUser.Building = building;
            retrivedUser.Roles.Add(role);

            if (role.Name.Equals(UserRoleConstant.Provider))
            {
                retrivedUser.Badge = (await _badgeRepository.FindAsync(x => x.IsActive && x.BadgeName == BadgeConstant.PromisingProvider))
                    .FirstOrDefault() ?? throw new BadgeNotFoundException(BadgeConstant.PromisingProvider);
            }

            await _vigigUserRepository.AddAsync(retrivedUser);
        }
        
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true) { Data = _mapper.Map<RegisterResponse>(retrivedUser)};
    }

    public async Task<ServiceActionResult> LoginAsync(LoginRequest request)
    {
        var retrivedUser = (await _vigigUserRepository.FindAsync(c => c.Email.Equals(request.Email)))
            .Include(x => x.Roles)  
            .FirstOrDefault();

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
            // throw new RefreshTokenNotFoundException(token.RefreshToken);
            throw new Exception("Not found token.");
        var customer = (await _vigigUserRepository.FindAsync(c => c.Id == refreshToken.UserId))
            .Include(x => x.Roles)
            .FirstOrDefault();
        if (customer is null)
            throw new UserNotFoundException(refreshToken.UserId.ToString());
        var tokenResponse = await GenerateAuthResponseAsync(customer);
        return new ServiceActionResult()
        {
            Data = tokenResponse
        };
    }

    public async Task<AuthResponse> GenerateAuthResponseAsync(VigigUser vigigUser)
    {
        var roles = vigigUser.Roles.Select(role => role.Name).ToList();

        var reponse = new AuthResponse()
        {
            Name = vigigUser.UserName ?? vigigUser.Email ?? String.Empty,
            Roles = roles ,
            Token = new TokenResponse()
            {
                AccessToken = _jwtService.GenerateAccessToken(vigigUser,roles),
                RefreshToken = await _jwtService.GenerateRefreshToken(vigigUser.Id),
                ExpiresAt = DateTimeOffset.Now.AddMinutes(_jwtSetting.RefreshTokenLifetimeInMinutes)
            }
        };
        return reponse;
    }
}