﻿using Microsoft.OpenApi.Models;
using Vigig.Common.Helpers;

namespace Vigig.Common.Settings;

public class SwaggerSetting
{
 	public required string Version { get; set; } 
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public required string TermsOfServiceUrl { get; set; } 
    public required string ContactName { get; set; } 
    public required string ContactEmail { get; set; } 
    public required string ContactUrl { get; set; } 
    public required string LicenseName { get; set; } 
    public required string LicenseUrl { get; set; } 
    public SwaggerOptions Options { get; set; } = new();

    public OpenApiContact GetContact()
    {
        return new OpenApiContact
        {
            Name = ContactName,
            Url = new Uri(ContactUrl),
            Email = ContactEmail
        };
    }

    public OpenApiLicense GetLicense()
    {
        return new OpenApiLicense
        {
            Name = LicenseName,
            Url = new Uri(LicenseUrl)
        };
    }

    public Uri GetTermsOfService()
    {
        return new Uri(TermsOfServiceUrl);
    }

    public List<OpenApiServer> GetServers()
    {
        return Options.Servers.Where(s => !string.IsNullOrEmpty(s.Url)).Select(s => new OpenApiServer
        {
            Url = s.Url,
            Description = s.Description,
            Variables = s.Variables.Any() 
                ? s.Variables.ToDictionary(
                    v => v.Name,
                    v => new OpenApiServerVariable()
                    {
                        Description = v.Description,
                        Default = v.DefaultValue
                    }) 
                : new Dictionary<string, OpenApiServerVariable>()
            
        })
        .ToList();
    }

    public OpenApiSecurityScheme GetSecurityScheme()
    {
        var securityScheme = Options.SecurityScheme;
        return new OpenApiSecurityScheme
        {
            Name = securityScheme.Name,
            Description = securityScheme.Description,
            Type = securityScheme.GetSecuritySchemeType(),
            In = securityScheme.GetParameterLocation(),
            BearerFormat = "JWT",
            Scheme = "Bearer"
        };
    }
    // In = ParameterLocation.Header,
    // Description = "Please enter a valid token",
    // Name = "Authorization",
    // Type = SecuritySchemeType.Http,
    // BearerFormat = "JWT",
    // Scheme = "Bearer"
    public OpenApiSecurityRequirement GetSecurityRequirement()
    {
        var securityRequirement = Options.SecurityRequirement;
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = securityRequirement.GetReferenceType(),
                        Id = securityRequirement.Id
                    }
                },
                Array.Empty<string>()
            }
        };
    }
}

public class SwaggerOptions
{
    public List<SwaggerServer> Servers { get; set; } = new();
    public SwaggerSecurityScheme SecurityScheme { get; set; }
    public SwaggerSecurityRequirement SecurityRequirement { get; set; }
}

public class SwaggerServer
{
    public string Url { get; set; } 
    public string Description { get; set; } 
    public List<SwaggerServerVariable> Variables { get; set; } = new();
}

public class SwaggerServerVariable
{
    public string Name { get; set; } 
    public string Description { get; set; } 
    public string DefaultValue { get; set; } 
}

public class SwaggerSecurityScheme
{
    public string Name { get; set; } 
    public string Description { get; set; } 
    public string Type { get; set; } 
    public string Location { get; set; } 

    public SecuritySchemeType GetSecuritySchemeType()
    {
        return EnumHelper.GetEnumValueFromString<SecuritySchemeType>(Type);
    }

    public ParameterLocation GetParameterLocation()
    {
        return EnumHelper.GetEnumValueFromString<ParameterLocation>(Location);
    }
}

public class SwaggerSecurityRequirement
{
    public string Type { get; set; } 
    public string Id { get; set; } 

    public ReferenceType GetReferenceType()
    {
        return EnumHelper.GetEnumValueFromString<ReferenceType>(Type);
    }
}