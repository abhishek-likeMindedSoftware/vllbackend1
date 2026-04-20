namespace LemonLaw.Application.Interfaces;

public interface IDocumentStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string mimeType, Guid applicationId, Guid documentId);
    Task<Stream> DownloadAsync(string storagePath);
    Task DeleteAsync(string storagePath);
}
