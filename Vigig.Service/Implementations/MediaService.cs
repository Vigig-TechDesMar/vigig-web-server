using System.Collections.ObjectModel;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Vigig.Common.Exceptions;
using Vigig.Common.Helpers;
using Vigig.Common.Settings;
using Vigig.Service.Interfaces;

namespace Vigig.Service.Implementations;

public class MediaService : IMediaService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _config;
    private readonly AzureSetting _azureSetting;

    public MediaService(IConfiguration config)
    {
        _config = config;
        _azureSetting = config.GetSection(nameof(AzureSetting)).Get<AzureSetting>() ?? throw new MissingAzureSettingException();
        _blobServiceClient = new BlobServiceClient(_azureSetting.AzureBlobStorage);
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSetting.BlobContainer);
        var blobInstance =
            containerInstance.GetBlobClient(file.FileName);
        await blobInstance.UploadAsync(file.OpenReadStream());
        return blobInstance.Uri.ToString();
    }

    public async Task<Stream> GetFile(string fileName)
    {
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSetting.BlobContainer);
        var blobInstance = containerInstance.GetBlobClient(StringInterpolationHelper.GenerateUniqueFileName(fileName,10));
        var downloadResult = await blobInstance.DownloadAsync();
        return downloadResult.Value.Content;
    }

    public async Task<ICollection<string>> GetUrlAfterUploadingFile(List<IFormFile> files)
    {
        var listUrl = new List<string>();
        var containerInstance = _blobServiceClient.GetBlobContainerClient(_azureSetting.BlobContainer);
        
        foreach (var file in files)
        {
            var blobInstance = containerInstance.GetBlobClient(StringInterpolationHelper.GenerateUniqueFileName(file.FileName,10));
            await blobInstance.UploadAsync(file.OpenReadStream());
            listUrl.Add(blobInstance.Uri.ToString());
        }
        return listUrl;
    }
}