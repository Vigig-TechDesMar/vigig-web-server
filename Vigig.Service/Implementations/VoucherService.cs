using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Voucher;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.Voucher;

namespace Vigig.Service.Implementations;

public class VoucherService : IVoucherService
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public VoucherService(IVoucherRepository voucherRepository, IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _voucherRepository = voucherRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var vouchers = _mapper.ProjectTo<DtoVoucher>(await _voucherRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = vouchers
        };
    }

    public async Task<ServiceActionResult> GetById(Guid id)
    {
        var voucher = (await _voucherRepository.FindAsync(sc => sc.Id == id && sc.IsActive)).FirstOrDefault()
                      ?? throw new VoucherNotFoundException(id.ToString(),nameof(Voucher.Id));

        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoVoucher>(voucher)
        };
    }
    
    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var vouchers = _mapper.ProjectTo<DtoVoucher>(
            await _voucherRepository.FindAsync(sc=> sc.IsActive));
        var paginatedResult =
            PaginationHelper.BuildPaginatedResult<DtoVoucher>(vouchers, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedResult
        };
    }

    public async Task<ServiceActionResult> SearchVoucher(SearchUsingGet request)
    {
        var vouchers = (await _voucherRepository.GetAllAsync()).AsEnumerable();
        var searchResults =
            _mapper.Map<IEnumerable<DtoVoucher>>(SearchHelper.BuildSearchResult<Voucher>(vouchers, request));
        var paginatedresults =
            PaginationHelper.BuildPaginatedResult(searchResults.AsQueryable(), request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = paginatedresults
        };
    }

    public async Task<ServiceActionResult> AddAsync(CreateVoucherRequest request)
    {
        //Check Event
        if(!await _eventRepository.ExistsAsync(sc=> sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));
        
        var voucher = _mapper.Map<Voucher>(request);
        await _voucherRepository.AddAsync(voucher);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoVoucher>(voucher),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateVoucherRequest request)
    {
        //Check Event
        if(!await _eventRepository.ExistsAsync(sc=> sc.Id == request.EventId && sc.IsActive))
            throw new EventNotFoundException(request.EventId,nameof(Event.Id));

        var voucher = (await _voucherRepository.FindAsync(sc => sc.Id == request.Id && sc.IsActive)).FirstOrDefault()
                      ?? throw new VoucherNotFoundException(request.Id,nameof(Voucher.Id));
        
        _mapper.Map(request, voucher);
        await _voucherRepository.UpdateAsync(voucher);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoVoucher>(voucher),
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public async Task<ServiceActionResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}