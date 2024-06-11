using Vigig.Domain.Entities;
using Vigig.Service.Interfaces;
using Vigig.Service.Models.Common;
using Vigig.Service.Models.Email;

namespace Vigig.Service.Implementations;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly SmtpSettings? _smtpSettings;

    public EmailService(IConfiguration configuration)
    {
        _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        if (_smtpSettings is null)
            throw new Exception("No STMP Settings");

        Console.Write("Sending to " + toEmail);
        
        var smtpClient = new SmtpClient(_smtpSettings.Server)
        {
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendEmailToUsersAsync(List<VigigUser> users, string subject, string body)
    {
        if (_smtpSettings is null)
            throw new Exception("No STMP Settings");
        
        var smtpClient = new SmtpClient(_smtpSettings.Server)
        {
            Port = _smtpSettings.Port,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        foreach (var x in users)
        {
            mailMessage.To.Add(x.Email);
        }
        await smtpClient.SendMailAsync(mailMessage);
    }
}

