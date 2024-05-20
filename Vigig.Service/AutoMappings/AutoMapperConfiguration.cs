using AutoMapper;
using Vigig.Domain.Dtos;
using Vigig.Domain.Dtos.Badge;
using Vigig.Domain.Dtos.Building;
using Vigig.Domain.Dtos.Service;
using Vigig.Domain.Dtos.VigigUser;
using Vigig.Domain.Entities;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Request.Badge;
using Vigig.Service.Models.Request.Building;
using Vigig.Service.Models.Request.GigService;
using Vigig.Service.Models.Request.Service;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
        CreateUserMaps(mapper);
        CreateBuildingMaps(mapper);
        CreateServiceMaps(mapper);
        CreateBadgeMaps(mapper);
        CreateVigigUserMaps(mapper);
    }

    public static void CreateUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<RegisterRequest, VigigUser>();
        mapper.CreateMap<VigigUser,RegisterResponse>();
    }

    public static void CreateBuildingMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Building, DtoBuilding>();
        mapper.CreateMap<CreateBuildingRequest, Building>();
    }

    public static void CreateServiceMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<GigServiceRequest, GigService>();
        mapper.CreateMap<ServiceCategoryRequest, ServiceCategory>();
       
        mapper.CreateMap<GigService, DtoGigService>();
        mapper.CreateMap<ServiceCategory,DtoServiceCategory>();
        mapper.CreateMap<UpdateServiceCategoryRequest, ServiceCategory>()
            .ForMember(sc => sc.Description, opt => opt.Condition( c => !string.IsNullOrWhiteSpace(c.Description)))
            .ForMember(sc => sc.CategoryName, opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.CategoryName)));

        mapper.CreateMap<UpdateGigServiceRequest, GigService>()
            .ForMember(s => s.Description ,opt => opt.Condition(r => !string.IsNullOrWhiteSpace(r.Description)))
            .ForMember(s => s.ServiceName ,opt => opt.Condition(r => !string.IsNullOrWhiteSpace(r.ServiceName)));
    }

    public static void CreateBadgeMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<CreateBadgeRequest, Badge>();
        mapper.CreateMap<UpdateBadgeRequest, Badge>()
            .ForMember(b => b.BadgeName,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.BadgeName)))
            .ForMember(b => b.Description,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.Description)))
            .ForMember(b => b.Benefit,opt => opt.Condition(c => !string.IsNullOrWhiteSpace(c.Benefit)));
        mapper.CreateMap<Badge, DtoBadge>();
        mapper.CreateMap<Badge, DtoBadgeWithStatus>();

    }

    public static void CreateVigigUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<VigigUser, DtoUserProfile>();
    }

}