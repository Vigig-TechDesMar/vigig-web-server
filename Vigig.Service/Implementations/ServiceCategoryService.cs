using AutoMapper;
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

    public async Task<ServiceActionResult> GetAllByIdAsync(Guid serviceCategoryId)
    {
        var category = (await _serviceCategoryRepository.FindAsync(sc => sc.Id == serviceCategoryId && sc.IsActive))
            .FirstOrDefault() ?? throw new ServiceCategoryNotFoundException(serviceCategoryId.ToString());
        return new ServiceActionResult(true)
        {
            Data = category
        };
    }

    public async Task<ServiceActionResult> AddAsync(ServiceCategoryRequest request)
    {
        var existedCategory = (await _serviceCategoryRepository.FindAsync(sc => sc.CategoryName.Equals(request.CategoryName) && sc.IsActive))
            .FirstOrDefault() ?? throw new ServiceCategoryAlreadyExistException(request.CategoryName,nameof(ServiceCategory.CategoryName));
        existedCategory = _mapper.Map<ServiceCategory>(existedCategory);
        await _serviceCategoryRepository.AddAsync(existedCategory);
        await _unitOfWork.CommitAsync();
        return new ServiceActionResult(true)
        {
            Data = existedCategory,
            StatusCode = StatusCodes.Status201Created
        };
    }

    public async Task<ServiceActionResult> UpdateAsync(ServiceCategoryRequest request)
    {
        var existedCategory = (await _serviceCategoryRepository.FindAsync(sc => sc.CategoryName.Equals(request.CategoryName) && sc.IsActive))
            .FirstOrDefault() ?? throw new ServiceCategoryAlreadyExistException(request.CategoryName,nameof(ServiceCategory.CategoryName));
        _mapper.Map(request,existedCategory);
        await _serviceCategoryRepository.AddAsync(existedCategory);
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
}