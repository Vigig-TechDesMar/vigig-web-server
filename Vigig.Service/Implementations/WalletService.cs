using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Wallet;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Wallet;

namespace Vigig.Service.Implementations;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository walletRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _walletRepository = walletRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var wallets = _mapper.ProjectTo<Wallet>(await _walletRepository.GetAllAsync());
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