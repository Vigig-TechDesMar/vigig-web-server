namespace Vigig.Service.Models.Email;

public class SmtpSettings
{
    public string Server { get; set; } = String.Empty;
    public int Port { get; set; }
    public string SenderName { get; set; } = String.Empty;
    public string SenderEmail { get; set; } = String.Empty;
    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}