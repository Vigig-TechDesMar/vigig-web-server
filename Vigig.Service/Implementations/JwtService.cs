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
using Vigig.Service.Interfaces;
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

    public string GenerateAccessToken(VigigUser vigigUser, ICollection<VigigRole> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey));
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email,vigigUser.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Sub,vigigUser.Id.ToString() ?? ""),
            new Claim(JwtRegisteredClaimNames.Name,vigigUser.FullName ?? ""),
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
        var existingRefreshToken = await _userTokenRepository.GetAsync(token =>
            token.Name == TokenTypeConstants.RefreshToken &&
                token.LoginProvider == LoginProviderConstants.VigigApp &&
                token.UserId == customerId);
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
}