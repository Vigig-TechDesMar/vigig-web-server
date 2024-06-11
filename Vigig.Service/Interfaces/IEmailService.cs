
using Vigig.Domain.Entities;

namespace Vigig.Service.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);

    Task SendEmailToUsersAsync(List<VigigUser> users, string subject, string body);
}