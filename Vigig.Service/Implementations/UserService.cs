using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.VigigUser;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Implementations;

public class UserService : IUserService
{
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IProviderServiceRepository _providerServiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public UserService(IVigigUserRepository vigigUserRepository, IProviderServiceRepository providerServiceRepository, IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService)
    {
        _vigigUserRepository = vigigUserRepository;
        _providerServiceRepository = providerServiceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<ServiceActionResult> GetProfileInformation(string token)
    {
        var isValidToken = _jwtService.IsValidToken(token);
        if (!isValidToken)
            throw new InvalidTokenException();
        var userId = _jwtService.GetTokenClaim(token, TokenClaimConstant.Subject)?.ToString();
        var userInfo = _mapper.Map<DtoUserProfile>((await _vigigUserRepository.FindAsync(x => x.Id.ToString() == userId && x.IsActive))
            .FirstOrDefault()) ?? throw new UserNotFoundException(userId);
        return new ServiceActionResult(true)
        {
            Data = userInfo
        };
    }

    public Task<ServiceActionResult> UploadService(CreateProviderServiceRequest request)
    {
        throw new NotImplementedException();
    }
}