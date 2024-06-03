using System.Drawing.Printing;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Fees;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Fees;

namespace Vigig.Service.Implementations;

public class DepositService : IDepositService
{
    private readonly IDepositRepository _depositRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepositService(IDepositRepository depositRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _depositRepository = depositRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var deposits = _mapper.ProjectTo<Deposit>(await _depositRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = deposits
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var deposit = (await _depositRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault();
        if (deposit is null)
            throw new DepositNotFoundException(id.ToString());
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoDeposit>(deposit)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var deposits = _mapper.ProjectTo<DtoDeposit>(
            await _depositRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoDeposit>(deposits, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchDeposit(SearchUsingGet request)
    {
        var deposits = (await _depositRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoDeposit>>(SearchHelper.BuildSearchResult<Deposit>(deposits, request));
        var paginatedResults = PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize,request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateDepositRequest request)
    {
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId);
        var deposit = _mapper.Map<Deposit>(request);
        await _depositRepository.AddAsync(deposit);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoDeposit>(deposit),
            StatusCode = StatusCodes.Status201Created
        };

    }

    //Admin
    public async Task<ServiceActionResult> UpdateAsync(UpdateDepositRequest request)
    {
        if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.ProviderId && sc.IsActive))
            throw new UserNotFoundException(request.ProviderId);
        var deposit = (await _depositRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault() ??
                      throw new DepositNotFoundException(request.Id);
        _mapper.Map(request,deposit);
        await _depositRepository.UpdateAsync(deposit);
        await _unitOfWork.CommitAsync();
        
        return new ServiceActionResult(true)
        {
            Data = deposit,
            StatusCode = StatusCodes.Status204NoContent
        };

    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}