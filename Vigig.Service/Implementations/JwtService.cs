using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vigig.Common.Constants.Validations;
using Vigig.Common.Settings;
using Vigig.Common.Exceptions;
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
    private readonly ICustomerTokenRepository _customerTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    

    public JwtService( ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IConfiguration configuration, ICustomerTokenRepository customerTokenRepository)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _customerTokenRepository = customerTokenRepository;
        _jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() ?? throw new MissingJwtSettingsException();
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

    public async Task<string> GenerateRefreshToken(Guid customerId)
    {
        var existingRefreshToken = await _customerTokenRepository.GetAsync(token =>
            token.Name == TokenTypeConstants.RefreshToken &&
                token.LoginProvider == LoginProviderConstants.VigigApp &&
                token.CustomerId == customerId);
        if (existingRefreshToken is null)
        {
            existingRefreshToken = new CustomerToken()
            {
                Name = TokenTypeConstants.RefreshToken,
                CustomerId = customerId,
                LoginProvider = LoginProviderConstants.VigigApp,
                Value = Guid.NewGuid().ToString()
            };
            await _customerTokenRepository.AddAsync(existingRefreshToken);
        }
        else
        {
            existingRefreshToken.Value = Guid.NewGuid().ToString();
            await _customerTokenRepository.UpdateAsync(existingRefreshToken);
        }

        await _unitOfWork.CommitAsync();
        return existingRefreshToken.Value;
    }
}