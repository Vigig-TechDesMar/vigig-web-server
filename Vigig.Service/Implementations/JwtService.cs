using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vigig.Common.Exceptions;
using Vigig.Common.Settings;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Models;
using Vigig.Service.Enums;
using Vigig.Service.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Vigig.Service.Implementations;

public class JwtService : IJwtService
{
    private readonly JwtSetting _jwtSetting;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JwtService(JwtSetting jwtSetting, ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() ?? throw new MissingJwtSettingsException();
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public string GenerateAccessToken(Customer customer)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SigningKey));
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email,customer.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Sub,customer.Id.ToString() ?? ""),
            new Claim(JwtRegisteredClaimNames.Name,customer.FullName ?? ""),
            new Claim(ClaimTypes.Role,UserRole.Client.ToString())
        };
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

    public string GenerateRefreshToken(Guid customerId)
    {
        throw new NotImplementedException();
    }
}