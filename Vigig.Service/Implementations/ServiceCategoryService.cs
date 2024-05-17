using AutoMapper;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Helpers;
using Vigig.DAL.Interfaces;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Entities;
using Vigig.Service.Exceptions.AlreadyExist;
using Vigig.Service.Exceptions.NotFound;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Request.Service;

namespace Vigig.Service.Implementations;

public class ServiceCategoryService : IServiceCategoryService
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ServiceCategoryService(IServiceCategoryRepository serviceCategoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ServiceActionResult> GetAllAsync()
    {
        var categories = _mapper.ProjectTo<DtoServiceCategory>(await _serviceCategoryRepository.GetAllAsync());
        return new ServiceActionResult(true)
        {
            Data = categories
        };
    }

    public async Task<ServiceActionResult> GetByIdAsync(Guid serviceCategoryId)
    {
        var category = (await _serviceCategoryRepository.FindAsync(sc => sc.Id == serviceCategoryId && sc.IsActive))
            .FirstOrDefault() ?? throw new ServiceCategoryNotFoundException(serviceCategoryId.ToString());
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoServiceCategory>(category)
        };
    }

    public async Task<ServiceActionResult> AddAsync(ServiceCategoryRequest request)
    {
        var existedCategory = (await _serviceCategoryRepository.FindAsync(sc => sc.CategoryName.Equals(request.CategoryName) && sc.IsActive))
            .FirstOrDefault();
        if (existedCategory is not null)    
            throw new ServiceCategoryAlreadyExistException(request.CategoryName,nameof(ServiceCategory.CategoryName));
        existedCategory = _mapper.Map<ServiceCategory>(request);
        await _serviceCategoryRepository.AddAsync(existedCategory);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = _mapper.Map<DtoServiceCategory>(existedCategory),
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(UpdateServiceCategoryRequest request)
    {
        var existedCategory = (await _serviceCategoryRepository.FindAsync(sc => sc.Id == request.Id && sc.IsActive))
            .FirstOrDefault() ?? throw new ServiceCategoryNotFoundException(request.Id);
        
        _mapper.Map(request,existedCategory);
        await _serviceCategoryRepository.UpdateAsync(existedCategory);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            StatusCode = StatusCodes.Status204NoContent
        };
    }

    public Task<ServiceActionResult> DeactivateAsync(Guid serviceCategoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceActionResult> GetPaginatedResultAsync(BasePaginatedRequest request)
    {
        var categories = _mapper.ProjectTo<DtoServiceCategory>(await _serviceCategoryRepository.GetAllAsync());
        var paginatedResult = PaginationHelper.BuildPaginatedResult(categories, request.PageSize, request.PageIndex);
        return new ServiceActionResult(true)
        {
            Data = categories
        };
    }
}