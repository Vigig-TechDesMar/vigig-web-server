﻿namespace Vigig.Domain.Dtos.VigigUser;

public class DtoUserProfile
{
    public string UserName { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public string? Address { get; set; }

    public string? Email { get; set; }
    
    public string? ProfileImage { get; set; }
    
    public string? Gender { get; set; }
    
    public string? FullName { get; set; }
    
    public Guid BuildingId { get; set; }
}