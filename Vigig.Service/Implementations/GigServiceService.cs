﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Models;
using Vigig.Service.Exceptions.AlreadyExist;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models;
using Vigig.Service.Models.Request.GigService;

namespace Vigig.Service.Implementations;

public class GigServiceService : IGigServiceService
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IGigServiceRepository _gigServiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GigServiceService(IGigServiceRepository gigServiceRepository, IServiceCategoryRepository serviceCategoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
        _gigServiceRepository = gigServiceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var services = _mapper.ProjectTo<DtoGigService>(await _gigServiceRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = services
        };
    }

    public async Task<ServiceActionResult> GetById(Guid gigId)
    {
        var existedService = (await _gigServiceRepository.FindAsync(s => s.Id == gigId && s.IsActive))
            .FirstOrDefault() ?? throw new GigServiceAlreadyExistException(gigId.ToString(), nameof(GigService.Id) );
        var service = _mapper.Map<DtoGigService>(existedService);
        return new ServiceActionResult(true)
        {
            Data = service
        };
    }

    public async Task<ServiceActionResult> AddAsync(GigServiceRequest request)
    {
        if (!await _serviceCategoryRepository.ExistsAsync(c => c.Id == request.ServiceCategoryId && c.IsActive))
             throw new ServiceCategoryNotFoundException(request.ServiceCategoryId);
        var service = _mapper.Map<GigService>(request);
        await _gigServiceRepository.AddAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = service,
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(GigServiceRequest request)
    {
        if (!await _serviceCategoryRepository.ExistsAsync(c => c.Id == request.ServiceCategoryId && c.IsActive))
            throw new ServiceCategoryNotFoundException(request.ServiceCategoryId);
        var service = _mapper.Map<GigService>(request);
        await _gigServiceRepository.UpdateAsync(service);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public Task<ServiceActionResult> DeactivateAsync(Guid gigId)
    {
        throw new NotImplementedException();
    }
    
    
}