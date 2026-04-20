using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LemonLaw.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Infrastructure.Services;

public class AzureBlobStorageService(
    IConfiguration configuration,
    ILogger<AzureBlobStorageService> logger) : IDocumentStorageService
{
    private BlobContainerClient GetContainerClient()
    {
        var connectionString = configuration["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureStorage:ConnectionString is not configured.");
        var containerName = configuration["AzureStorage:ContainerName"] ?? "lemonlaw-documents";
        return new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string> UploadAsync(
        Stream fileStream, string fileName, string mimeType, Guid applicationId, Guid documentId)
    {
        var container = GetContainerClient();
        await container.CreateIfNotExistsAsync(PublicAccessType.None);

        var blobPath = $"{applicationId}/{documentId}/{fileName}";
        var blobClient = container.GetBlobClient(blobPath);

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = mimeType }
        };

        await blobClient.UploadAsync(fileStream, uploadOptions);
        logger.LogInformation("Uploaded blob '{Path}'.", blobPath);

        return blobPath;
    }

    public async Task<Stream> DownloadAsync(string storagePath)
    {
        var container = GetContainerClient();
        var blobClient = container.GetBlobClient(storagePath);
        var response = await blobClient.DownloadStreamingAsync();
        return response.Value.Content;
    }

    public async Task DeleteAsync(string storagePath)
    {
        var container = GetContainerClient();
        var blobClient = container.GetBlobClient(storagePath);
        await blobClient.DeleteIfExistsAsync();
        logger.LogInformation("Deleted blob '{Path}'.", storagePath);
    }
}
