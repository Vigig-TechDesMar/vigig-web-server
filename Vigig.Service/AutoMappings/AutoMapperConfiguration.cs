using AutoMapper;
using Vigig.Domain.Models;
using Vigig.Service.Models.Request.Authentication;
using Vigig.Service.Models.Response.Authentication;

namespace Vigig.Service.AutoMappings;

public static class AutoMapperConfiguration
{
    public static void RegisterMaps(IMapperConfigurationExpression mapper)
    {
        CreateUserMaps(mapper);
    }

    public static void CreateUserMaps(IMapperConfigurationExpression mapper)
    {
        mapper.CreateMap<RegisterRequest, Customer>();
        mapper.CreateMap<Customer,RegisterResponse>();

    }


}