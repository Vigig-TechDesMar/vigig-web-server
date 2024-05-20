namespace Vigig.Domain.Models.BaseEntities;

public class IdentityEntity : BaseEntity<Guid>
{ 
    public string UserName { get; set; } = null!;
    
    public string NormalizedUserName { get; set; } = null!;
    
    public string? Phone { get; set; }
    

    public string Password { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    

    public string NormalizedEmail { get; set; } = null!;
    
    public bool? EmailConfirmed { get; set; }
    
    
    public string? ConcurrencyStamp { get; set; }
    
    public string? Gender { get; set; }
    

    public string? ProfileImage { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Address { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public bool IsActive { get; set; }
}