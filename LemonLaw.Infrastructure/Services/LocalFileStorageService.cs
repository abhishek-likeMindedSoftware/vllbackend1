using LemonLaw.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Infrastructure.Services;

/// <summary>
/// Local disk storage for development — used when AzureStorage:UseLocal = true.
/// Files are stored under the configured LocalStoragePath (default: ./uploads).
/// </summary>
public class LocalFileStorageService(
    IConfiguration configuration,
    ILogger<LocalFileStorageService> logger) : IDocumentStorageService
{
    private string GetBasePath()
    {
        var path = configuration["AzureStorage:LocalStoragePath"] ?? "uploads";
        // Make absolute relative to the app's content root
        if (!Path.IsPathRooted(path))
            path = Path.Combine(AppContext.BaseDirectory, path);
        Directory.CreateDirectory(path);
        return path;
    }

    public async Task<string> UploadAsync(
        Stream fileStream, string fileName, string mimeType, Guid applicationId, Guid documentId)
    {
        var basePath = GetBasePath();
        var relativePath = Path.Combine(applicationId.ToString(), documentId.ToString());
        var fullDir = Path.Combine(basePath, relativePath);
        Directory.CreateDirectory(fullDir);

        // Sanitise the filename
        var safeFileName = Path.GetFileName(fileName);
        var fullPath = Path.Combine(fullDir, safeFileName);

        await using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(fs);

        var storagePath = Path.Combine(relativePath, safeFileName).Replace('\\', '/');
        logger.LogInformation("Saved file locally at '{Path}'.", storagePath);
        return storagePath;
    }

    public Task<Stream> DownloadAsync(string storagePath)
    {
        var basePath = GetBasePath();
        var fullPath = Path.Combine(basePath, storagePath.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"File not found: {storagePath}");
        Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string storagePath)
    {
        var basePath = GetBasePath();
        var fullPath = Path.Combine(basePath, storagePath.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            logger.LogInformation("Deleted local file '{Path}'.", storagePath);
        }
        return Task.CompletedTask;
    }
}
