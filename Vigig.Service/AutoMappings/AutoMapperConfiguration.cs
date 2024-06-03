using AutoMapper;
using Vigig.Domain.Dtos;
using Vigig.Domain.Dtos.Badge;
using Vigig.Domain.Dtos.Booking;
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
        CreateBookingMaps(mapper);
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
        mapper.CreateMap<ProviderService, DtoProviderService>()
            .ForMember(dto => dto.ServiceName, opt => opt.MapFrom(x => x.Service.ServiceName))
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.Provider.UserName));
        mapper.CreateMap<ServiceImage, DtoServiceImage>();
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

    public static void CreateBookingMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<Booking, DtoPlacedBooking>()
        .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
        .ForMember(dto => dto.ProviderServiceName, opt => opt.MapFrom(x => x.ProviderService.Service.ServiceName))
        .ForMember(dto => dto.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName));
        mapper.CreateMap<Booking,DtoBooking>()
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
            .ForMember(dto => dto.ProviderServiceName, opt => opt.MapFrom(x => x.ProviderService.Service.ServiceName))
            .ForMember(dto => dto.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName));
        mapper.CreateMap<Booking, DtoBookChat>()
            .ForMember(dto => dto.ProviderName, opt => opt.MapFrom(x => x.ProviderService.Provider.UserName))
            .ForMember(dto => dto.ProviderProfileImage,
                opt => opt.MapFrom(x => x.ProviderService.Provider.ProfileImage))
            .ForMember(dto => dto.ClientName, opt => opt.MapFrom(x => x.VigigUser.UserName))
            .ForMember(dto => dto.ClientProfileImage, opt => opt.MapFrom(x => x.VigigUser.ProfileImage))
            .ForMember(dto => dto.ChatTitle, opt=> opt.MapFrom(x => x.ProviderService.Service.ServiceName +" - "+ x.CreatedDate.Date))
            .ForMember(dto => dto.LastMessage, opt =>
            {
                opt.Condition(x => x.BookingMessages.Any());
                opt.MapFrom(x => x.BookingMessages.OrderByDescending(x => x.SentAt).FirstOrDefault().Content);
            });
        mapper.CreateMap<BookingMessage, DtoBookingMessage>()
            .ForMember(dto => dto.SenderName, opt => opt.MapFrom(x => x.SenderName));
    }
}