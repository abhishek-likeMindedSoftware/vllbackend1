using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IDocumentService
{
    Task<CommonResponseDto<DocumentUploadResultDto>> UploadDocumentAsync(
        Guid applicationId, string documentType, string fileName,
        string mimeType, long fileSizeBytes, Stream fileStream, string uploadedByRole);

    Task<CommonResponseDto<bool>> DeleteDocumentAsync(Guid applicationId, Guid documentId);
}
