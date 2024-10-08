﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vigig.Common.Constants.Validations;
using Vigig.Common.Settings;
using Vigig.Common.Exceptions;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Vigig.Service.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    

    public JwtService( IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IConfiguration configuration, IUserTokenRepository userTokenRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _userTokenRepository = userTokenRepository;
        _jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() ?? throw new MissingJwtSettingsException();
    }

    public string GenerateAccessToken(VigigUser vigigUser, ICollection<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey));
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email,vigigUser.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Sub,vigigUser.Id.ToString() ?? ""),
            new Claim(JwtRegisteredClaimNames.Name,vigigUser.UserName ?? ""),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience,
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(_jwtSetting.AccessTokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken(Guid customerId)
    {
        var existingRefreshToken = (await _userTokenRepository.FindAsync(token =>
                token.Name == TokenTypeConstants.RefreshToken &&
                token.LoginProvider == LoginProviderConstants.VigigApp &&
                token.UserId == customerId)).FirstOrDefault();
        if (existingRefreshToken is null)
        {
            existingRefreshToken = new UserToken()
            {
                Name = TokenTypeConstants.RefreshToken,
                UserId = customerId,
                LoginProvider = LoginProviderConstants.VigigApp,
                Value = Guid.NewGuid().ToString()
            };
            await _userTokenRepository.AddAsync(existingRefreshToken);
        }
        else
        {
            existingRefreshToken.Value = Guid.NewGuid().ToString();
            await _userTokenRepository.UpdateAsync(existingRefreshToken);
        }

        await _unitOfWork.CommitAsync();
        return existingRefreshToken.Value;
    }

    public bool IsValidToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = _jwtSetting.Issuer,
            ValidateIssuer = _jwtSetting.ValidateIssuer,
            ValidAudience = _jwtSetting.Audience,
            ValidateAudience = _jwtSetting.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey)),
            ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSetting.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public object? GetTokenClaim(string token, string claimName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSetting.Issuer,
            ValidateIssuer = _jwtSetting.ValidateIssuer,
            ValidAudience = _jwtSetting.Audience,
            ValidateAudience = _jwtSetting.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey)),
            ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSetting.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            var propInfo = typeof(JwtSecurityToken).GetProperties().FirstOrDefault(p => p.Name == claimName);
            return propInfo?.GetValue(jwtSecurityToken);
        }
        catch
        {
            throw new InvalidTokenException();
        }
    }

    public string GetSubjectClaim(string token)
    {
        return GetTokenClaim(token, TokenClaimConstant.Subject).ToString() ?? throw new InvalidTokenException();
    }

    public object? GetRoleClaim(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSetting.Issuer,
            ValidateIssuer = _jwtSetting.ValidateIssuer,
            ValidAudience = _jwtSetting.Audience,
            ValidateAudience = _jwtSetting.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey)),
            ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSetting.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            return jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;
        }
        catch
        {
            throw new InvalidTokenException();
        }
    }

    public AuthModel GetAuthModel(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSetting.Issuer,
            ValidateIssuer = _jwtSetting.ValidateIssuer,
            ValidAudience = _jwtSetting.Audience,
            ValidateAudience = _jwtSetting.ValidateAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey)),
            ValidateIssuerSigningKey = _jwtSetting.ValidateIssuerSigningKey,
            ValidateLifetime = _jwtSetting.ValidateLifetime,
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            var jwtSecurityToken = (JwtSecurityToken)validatedToken;
            var role =  jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "role").Value;
            var userName = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "name").Value;
            var userId = jwtSecurityToken.Subject;
            return new AuthModel
            {
                UserId = new Guid(userId),
                UserName = userName,
                Role = role
            };
        }
        catch
        {
            throw new InvalidTokenException();
        }
    }
}