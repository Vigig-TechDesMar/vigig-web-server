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
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Implementations;

public class UserService : IUserService
{
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IProviderServiceRepository _providerServiceRepository;
    private readonly IMediaService _mediaService;
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public UserService(IVigigUserRepository vigigUserRepository, IProviderServiceRepository providerServiceRepository, IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService, IGigServiceRepository gigServiceRepository, IMediaService mediaService, IBuildingRepository buildingRepository)
    {
        _vigigUserRepository = vigigUserRepository;
        _providerServiceRepository = providerServiceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
        _gigServiceRepository = gigServiceRepository;
        _mediaService = mediaService;
        _buildingRepository = buildingRepository;
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

        var imageUrls = await _mediaService.GetUrlAfterUploadingFile(request.Images);
        
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
            RatingCount = 0,
        };

        await _providerServiceRepository.AddAsync(providerService);
        await _unitOfWork.CommitAsync();
        
        var serviceImages = imageUrls.Select(x => new ServiceImage
        {
            ImageUrl = x,
        }).ToList();
        
        providerService.ServiceImages = serviceImages;
        await _providerServiceRepository.UpdateAsync(providerService);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoProviderService>(providerService),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateProfile(string token, UpdateProfileRequest request)
    {
        var userId = _jwtService.GetAuthModel(token).UserId;
        if (request.BuildingId != default && !(await _buildingRepository.ExistsAsync(x => x.Id == request.BuildingId && x.IsActive)))
        {
            throw new BuildingNotFoundException(request.BuildingId, nameof(Building.Id));
        }
        var user = await _vigigUserRepository.GetAsync(x => x.Id == userId && x.IsActive) ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));
        user.Phone = (!string.IsNullOrEmpty(request.Phone)) ? request.Phone : user.Phone;
        user.Gender = (!string.IsNullOrEmpty(request.Gender)) ? request.Gender : user.Gender;
        user.FullName = (!string.IsNullOrEmpty(request.FullName)) ? request.FullName : user.FullName;
        user.Address = (!string.IsNullOrEmpty(request.Address)) ? request.Address : user.Address;
        user.BuildingId = request.BuildingId;
        if (request.ProfileImage is not null)
        {
            var imageUrl = await _mediaService.UploadFile(request.ProfileImage);
            user.ProfileImage = imageUrl;
        }
        await _vigigUserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoUserProfile>(user),
            StatusCode = StatusCodes.Status204NoContent
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