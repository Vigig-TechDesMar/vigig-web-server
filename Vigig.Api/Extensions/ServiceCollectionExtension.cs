using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vigig.Common.Attribute;
using Vigig.Common.Constants;
using Vigig.Common.Exceptions;
using Vigig.Common.Interfaces;
using Vigig.Common.Settings;
using Vigig.DAL.Data;
using Vigig.DAL.Implementations;
using Vigig.DAL.Interfaces;
using Vigig.Service.AutoMappings;



namespace Vigig.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        // services.AddScoped<>()
        services.AddScoped<IAppDbContext,VigigContext>();

        var registeredServiceAttribute = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsInterface && type.GetCustomAttributes<ServiceRegisterAttribute>().Any())
            .ToList();

        foreach (var type in registeredServiceAttribute)
        {
            var attribute = type.GetCustomAttributes<ServiceRegisterAttribute>().FirstOrDefault()
                ?? throw new Exception("Not found ServiceRegisterAttribute");
            
            var lifeTimeProperty =  attribute.GetType().GetProperty("LifeTime", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new Exception("Not found life time") ;
            
            var lifeTime = lifeTimeProperty.GetValue(attribute);
            if (lifeTime is ServiceLifetime l)
            {
                var implementType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .FirstOrDefault(t => type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                
                if (implementType is not null)
                    services.Add(new ServiceDescriptor(type,implementType,l ));
            }
        }
        var config = new MapperConfiguration(AutoMapperConfiguration.RegisterMaps);
        var mapper = config.CreateMapper();
        services.AddSingleton(mapper);
        return services;
    }
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration, SwaggerSetting swaggerSetting = default)
    {
        swaggerSetting ??= configuration.GetSection(nameof(SwaggerSetting)).Get<SwaggerSetting>() ?? throw new MissingSwaggerSettingException();
        
        services.AddSwaggerGen(
            options => 
            { 
                options.SwaggerDoc(swaggerSetting.Version, new OpenApiInfo
                {
                    Version = swaggerSetting.Version,
                    Title = swaggerSetting.Title,
                    Description = swaggerSetting.Description,
                    TermsOfService = swaggerSetting.GetTermsOfService(),
                    Contact = swaggerSetting.GetContact(),
                    License = swaggerSetting.GetLicense()
                });
                options.SwaggerGeneratorOptions = new SwaggerGeneratorOptions()
                {
                    Servers = swaggerSetting.GetServers()
                };
                options.AddSecurityDefinition(swaggerSetting.Options.SecurityScheme.Name, swaggerSetting.GetSecurityScheme());
                options.AddSecurityRequirement(swaggerSetting.GetSecurityRequirement());
            }
        );
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSetting = configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>() 
            ?? throw new MissingJwtSettingsException();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtSetting.Issuer,
                ValidateIssuer = jwtSetting.ValidateIssuer,
                ValidAudience = jwtSetting.Audience,
                ValidateAudience = jwtSetting.ValidateAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SigningKey)),
                ValidateIssuerSigningKey = jwtSetting.ValidateIssuerSigningKey,
                ClockSkew = TimeSpan.Zero
            };

            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/booking"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };

        });
        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSetting = configuration.GetSection(nameof(CorsSetting)).Get<CorsSetting>()
            ?? throw new  MissingCorsSettingsException();
        services.AddCors(opt =>
        {
            opt.AddPolicy(CorsConstant.APP_CORS_POLICY, builder =>
            {
                builder.WithOrigins(corsSetting.GetAllowedOriginsArray())
                    .WithHeaders(corsSetting.GetAllowedHeadersArray())
                    .WithMethods(corsSetting.GetAllowedMethodsArray());
                if (corsSetting.AllowCredentials)
                {
                    builder.AllowCredentials();
                }
                builder.Build();
            });
        });
        return services;
    }
}