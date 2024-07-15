using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Wallet;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Wallet;

namespace Vigig.Service.Implementations;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository walletRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService)
    {
        _walletRepository = walletRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var wallets = _mapper.ProjectTo<DtoWallet>(await _walletRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = wallets
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var wallet = (await _walletRepository.FindAsync(x => x.Id == id)).FirstOrDefault();
        if (wallet is null)
            throw new WalletNotFoundException(id.ToString(),nameof(Wallet.Id));
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoWallet>(wallet)
        };
    }

    public async Task<ServiceActionResult> GetOwnedWallet(string token)
    {
        var userId = _jwtService.GetSubjectClaim(token).ToString();
        var provider = (await _vigigUserRepository.FindAsync(x => x.IsActive && x.Id.ToString() == userId))
            .Include(x => x.Wallets)
            .FirstOrDefault() ?? throw new UserNotFoundException(userId,nameof(VigigUser.Id));

        //Get the wallet
        var wallet = provider.Wallets.FirstOrDefault();
        if (wallet is not null)
            return new ServiceActionResult(true)
            {
                Data = _mapper.Map<DtoWallet>(wallet)
            };
        var newWallet = new Wallet
        {
            Balance = 0,
            ProviderId = Guid.Parse(userId),
            CreatedDate = DateTime.Now,
        };
                
        await _walletRepository.AddAsync(newWallet);
        await _unitOfWork.CommitAsync();

        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoWallet>(newWallet)
        };
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var wallets = _mapper.ProjectTo<DtoWallet>(
            await _walletRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoWallet>(wallets, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchWallet(SearchUsingGet request)
    {
        var wallets = (await _walletRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoWallet>>(SearchHelper.BuildSearchResult<Wallet>(wallets, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    //Admin
    public async Task<ServiceActionResult> AddAsync(CreateWalletRequest request)
    {
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId,nameof(VigigUser.Id));
        var wallet = _mapper.Map<Wallet>(request);
        wallet.CreatedDate = DateTime.Now;
        await _walletRepository.AddAsync(wallet);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoWallet>(wallet),
            StatusCode = StatusCodes.Status201Created
        };
    }

    //Admin
    public async Task<ServiceActionResult> UpdateAsync(UpdateWalletRequest request)
    {
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId,nameof(VigigUser.Id));
        var wallet = (await _walletRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
                     ?? throw new WalletNotFoundException(request.Id,nameof(Wallet.Id));

        _mapper.Map(request, wallet);
        await _walletRepository.UpdateAsync(wallet);
        await _unitOfWork.CommitAsync();
        
        _mapper.Map<Wallet, DtoWallet>(wallet);
        return new ServiceActionResult(true)
        {
            Data = wallet,
            StatusCode = StatusCodes.Status204NoContent
        };

    }

    //Admin
    public async Task<ServiceActionResult> DeactivateAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}