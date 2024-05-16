namespace Vigig.Domain.Dtos.Service;

public class DtoServiceCategory
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}