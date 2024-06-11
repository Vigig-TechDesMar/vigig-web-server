namespace Vigig.Service.Models.Email;

public class EmailRequest
{
    public string ToEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}