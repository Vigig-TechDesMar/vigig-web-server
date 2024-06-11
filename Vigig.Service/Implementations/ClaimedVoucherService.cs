using System.Collections;
using System.Formats.Asn1;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Voucher;
using Vigig.Domain.Entities;
using Vigig.Service.Constants;
using Vigig.Service.Enums;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Voucher;

namespace Vigig.Service.Implementations;

public class ClaimedVoucherService : IClaimedVoucherService
{
    private readonly IClaimedVoucherRepository _claimedVoucherRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IVigigUserRepository _vigigUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClaimedVoucherService(IClaimedVoucherRepository claimedVoucherRepository, IVoucherRepository voucherRepository, IVigigUserRepository vigigUserRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _claimedVoucherRepository = claimedVoucherRepository;
        _voucherRepository = voucherRepository;
        _vigigUserRepository = vigigUserRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var claimedVoucher = _mapper.ProjectTo<DtoClaimedVoucher>(await _claimedVoucherRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = claimedVoucher
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        // var claimedVoucher = (await _claimedVoucherRepository.FindAsync(sc => sc.Id == id)).FirstOrDefault() ??
        //                       throw new ClaimedVoucherNotFoundException(id.ToString(),nameof(ClaimedVoucher.Id));
        // return new ServiceActionResult(true)
        // {
        //     Data = _mapper.Map<DtoClaimedVoucher>(claimedVoucher)
        // };
        throw new NotImplementedException();
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var claimedVouchers = _mapper.ProjectTo<DtoClaimedVoucher>(
            await _claimedVoucherRepository.GetAllAsync());
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoClaimedVoucher>(claimedVouchers, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchClaimedVoucher(SearchUsingGet request)
    {
        var claimedVouchers = (await _claimedVoucherRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoClaimedVoucher>>(
                SearchHelper.BuildSearchResult<ClaimedVoucher>(claimedVouchers, request));
        var paginatedResults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateClaimedVoucherRequest request)
    {
        //Check Voucher
        var voucher = (await _voucherRepository.FindAsync(sc => sc.Id == request.VoucherId && sc.IsActive)).FirstOrDefault()??
            throw new VoucherNotFoundException(request.VoucherId,nameof(Voucher.Id));

        //Single User Voucher
        if (request.CustomerId != null)
        {
            if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.CustomerId && sc.IsActive))
                throw new UserNotFoundException(request.CustomerId, nameof(VigigUser.Id));
            var customerId = request.CustomerId??new Guid();
            var claimedVoucher = new ClaimedVoucher
            {
                CustomerId = customerId,
                VoucherId = request.VoucherId,
                Voucher = voucher,
                Status = true
            };
            await _claimedVoucherRepository.AddAsync(claimedVoucher);
        }

        //Voucher for all Customers
        if (request.IsForAll)
        {
            var customerList = await _vigigUserRepository.FindAsync(sc =>
                sc.IsActive && sc.Roles.Contains<>(UserRole.Client));
            var claimedVoucherList = new List<ClaimedVoucher>();
            
            foreach (var x in customerList)
            {
                var claimedVoucher = new ClaimedVoucher
                {
                    CustomerId = x.Id,
                    Customer = x,
                    VoucherId = request.VoucherId,
                    Voucher = voucher,
                    Status = true
                };
                claimedVoucherList.Add(claimedVoucher);
                await _claimedVoucherRepository.AddManyAsync(claimedVoucherList);
            }
        }
        
        //Vouchers for a group of Users
        if (request.Badges != null && request.Badges.Length > 0)
        {
            foreach (var x in request.Badges)
            {
                
            }
        }
      
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateClaimedVoucherRequest request)
    {
        // //Check Voucher
        // if (!await _voucherRepository.ExistsAsync(sc => sc.Id == request.VoucherId && sc.IsActive))
        //     throw new VoucherNotFoundException(request.VoucherId,nameof(Voucher.Id));
        //
        // //Check User
        // if (!await _vigigUserRepository.ExistsAsync(sc => sc.Id == request.CustomerId && sc.IsActive))
        //     throw new UserNotFoundException(request.CustomerId,nameof(VigigUser.Id));
        //
        // var claimedVoucher = (await _claimedVoucherRepository.FindAsync(sc => sc.Id == request.Id)).FirstOrDefault()
        //                      ?? throw new ClaimedVoucherNotFoundException(request.Id,nameof(ClaimedVoucher.Id));
        //
        // _mapper.Map(request, claimedVoucher);
        // await _claimedVoucherRepository.UpdateAsync(claimedVoucher);
        // await _unitOfWork.CommitAsync();
        // return new ServiceActionResult(true)
        // {
        //     Data = _mapper.Map<DtoClaimedVoucher>(claimedVoucher),
        //     StatusCode = StatusCodes.Status204NoContent
        // };
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}