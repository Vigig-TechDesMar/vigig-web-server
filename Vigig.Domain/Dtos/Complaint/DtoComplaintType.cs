namespace Vigig.Domain.Dtos.Complaint;

public class DtoComplaintType
{
    public Guid Id;
    
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public bool IsActive { get; set; }
    
}