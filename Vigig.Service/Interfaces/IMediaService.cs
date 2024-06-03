using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;
using Vigig.Common.Attribute;

namespace Vigig.Service.Interfaces;
[ServiceRegister]
public interface IMediaService
{
    Task<string> UploadFile(IFormFile file);
    Task<Stream> GetFile(string fileName);
    Task<ICollection<string>> GetUrlAfterUploadingFile(List<IFormFile> files);
}