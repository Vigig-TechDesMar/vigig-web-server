using AutoMapper;
using Vigig.Domain.Dtos.Building;
using Vigig.Domain.Models;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Request.Building;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
        CreateUserMaps(mapper);
        CreateBuildingMaps(mapper);
        CreateServiceMaps(mapper);
    }

    public static void CreateUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<RegisterRequest, Customer>();
        mapper.CreateMap<Customer,RegisterResponse>();
    }

    public static void CreateBuildingMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Building, DtoBuilding>();
        mapper.CreateMap<CreateBuildingRequest, Building>();
    }

    public static void CreateServiceMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<CreateGigServiceRequest, GigService>();
        mapper.CreateMap<ServiceCategoryRequest, ServiceCategory>();
    }

}