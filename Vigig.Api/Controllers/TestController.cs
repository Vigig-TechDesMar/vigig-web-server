using Microsoft.AspNetCore.Authentication.JwtBearer;
using Vigig.Api.Controllers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vigig.Service.Implementations;
using Vigig.Service.Models.Email;

namespace Vigig.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TestController : BaseApiController
{
    private readonly EmailService _emailService;

    public TestController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        try
        {
            await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok("Email sent successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}