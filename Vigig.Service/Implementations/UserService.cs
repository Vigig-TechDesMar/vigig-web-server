using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Dtos.VigigUser;
using Vigig.Domain.Entities;
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
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public UserService(IVigigUserRepository vigigUserRepository, IProviderServiceRepository providerServiceRepository, IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService, IGigServiceRepository gigServiceRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _providerServiceRepository = providerServiceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
        _gigServiceRepository = gigServiceRepository;
    }

    public async Task<ServiceActionResult> GetProfileInformation(string token)
    {
        var userId = GetIdClaimFromToken(token);
        var userInfo = _mapper.Map<DtoUserProfile>((await _vigigUserRepository.FindAsync(x => x.Id.ToString() == userId && x.IsActive))
            .FirstOrDefault()) ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));
        return new ServiceActionResult(true)
        {
            Data = userInfo
        };
    }

    public async Task<ServiceActionResult> UploadService(string token, CreateProviderServiceRequest request)
    {
        var userId = GetIdClaimFromToken(token);
        
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == userId))
            .FirstOrDefault() ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));
        
        var gigService = (await _gigServiceRepository.FindAsync(x => x.IsActive && x.Id == request.ServiceId))
            .FirstOrDefault() ?? throw new GigServiceNotFoundException(request.ServiceId,nameof(GigService.Id));

        var providerService = new ProviderService()
        {
            Provider = provider,
            Service = gigService,
            Description = request.Description,
            StickerPrice = 
                (request.StickerPrice >= gigService.MinPrice && request.StickerPrice <= gigService.MaxPrice) 
                    ? request.StickerPrice : throw new PriceNotInRangeException(gigService.ServiceName),
            TotalBooking = 0,
            Rating = 0,
        };

        await _providerServiceRepository.AddAsync(providerService);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderService>(providerService),
            StatusCode = StatusCodes.Status201Created
        };
    }

    private string GetIdClaimFromToken(string token)
    {
        var isValidToken = _jwtService.IsValidToken(token);
        if (!isValidToken)
            throw new InvalidTokenException();
        var userId = _jwtService.GetTokenClaim(token, TokenClaimConstant.Subject)?.ToString();
        return userId;
    }
    
    
}