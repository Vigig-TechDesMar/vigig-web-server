using Microsoft.AspNetCore.Mvc;
using Vigig.Api.Controllers.Base;
using Vigig.Service.Interfaces;

namespace Vigig.Api.Controllers;
[Route("/api/[controller]")]
public class MediaController : BaseApiController
{
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpPost]
    public async Task UploadFile(IFormFile file)
    {
        await _mediaService.UploadFile(file);
    }
}