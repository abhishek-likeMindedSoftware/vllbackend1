using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

public class DocumentService(
    IDocumentStorageService storageService,
    IGenericRepository<ApplicationDocument> documentRepository,
    IGenericRepository<CaseEvent> eventRepository,
    ILogger<DocumentService> logger) : IDocumentService
{
    private const long MaxFileSizeBytes = 25 * 1024 * 1024; // 25 MB

    private static readonly HashSet<string> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "image/jpeg",
        "image/png",
        "image/tiff",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };

    public async Task<CommonResponseDto<DocumentUploadResultDto>> UploadDocumentAsync(
        Guid applicationId, string documentType, string fileName,
        string mimeType, long fileSizeBytes, Stream fileStream, string uploadedByRole)
    {
        try
        {
            if (fileSizeBytes > MaxFileSizeBytes)
                return Fail("File exceeds the maximum allowed size of 25 MB.");

            if (!AllowedMimeTypes.Contains(mimeType))
                return Fail($"File type '{mimeType}' is not permitted.");

            if (!Enum.TryParse<DocumentType>(documentType, ignoreCase: true, out var docType))
                return Fail($"Unknown document type '{documentType}'.");

            if (!Enum.TryParse<UploadedByRole>(uploadedByRole, ignoreCase: true, out var uploaderRole))
                return Fail($"Unknown uploader role '{uploadedByRole}'.");

            var documentId = Guid.NewGuid();
            var storagePath = await storageService.UploadAsync(fileStream, fileName, mimeType, applicationId, documentId);

            var document = new ApplicationDocument
            {
                Id = documentId,
                ApplicationId = applicationId,
                DocumentType = docType,
                FileName = fileName,
                StoragePath = storagePath,
                FileSizeBytes = fileSizeBytes,
                MimeType = mimeType,
                UploadedAt = DateTime.UtcNow,
                UploadedByRole = uploaderRole,
                Status = DocumentStatus.PENDING_REVIEW,
                VirusScanResult = VirusScanResult.PENDING
            };

            await documentRepository.AddAsync(document);

            await eventRepository.AddAsync(new CaseEvent
            {
                ApplicationId = applicationId,
                EventType = CaseEventType.DOCUMENT_UPLOADED,
                ActorType = uploaderRole == UploadedByRole.STAFF ? ActorType.STAFF
                          : uploaderRole == UploadedByRole.DEALER ? ActorType.DEALER
                          : ActorType.CONSUMER,
                ActorDisplayName = uploadedByRole,
                Description = $"Document '{fileName}' uploaded (type: {docType})."
            });

            await documentRepository.SaveChangesAsync();

            logger.LogInformation("Document {DocumentId} uploaded for application {ApplicationId}.", documentId, applicationId);

            return new CommonResponseDto<DocumentUploadResultDto>
            {
                Success = true,
                Data = new DocumentUploadResultDto
                {
                    DocumentId = documentId,
                    FileName = fileName,
                    StoragePath = storagePath,
                    Status = DocumentStatus.PENDING_REVIEW.ToString()
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading document for application {ApplicationId}.", applicationId);
            return Fail("Failed to upload document.");
        }
    }

    private static CommonResponseDto<DocumentUploadResultDto> Fail(string message) =>
        new() { Success = false, Message = message };

    public async Task<CommonResponseDto<bool>> DeleteDocumentAsync(Guid applicationId, Guid documentId)
    {
        try
        {
            var doc = await documentRepository.GetByIdAsync(documentId);
            if (doc == null || doc.ApplicationId != applicationId)
                return new CommonResponseDto<bool> { Success = false, Message = "Document not found." };
            if (doc.Status != DocumentStatus.PENDING_REVIEW)
                return new CommonResponseDto<bool> { Success = false, Message = "Only pending documents can be deleted." };
            await storageService.DeleteAsync(doc.StoragePath);
            documentRepository.Delete(doc);
            await documentRepository.SaveChangesAsync();
            return new CommonResponseDto<bool> { Success = true, Data = true };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting document {DocId}", documentId);
            return new CommonResponseDto<bool> { Success = false, Message = "Failed to delete document." };
        }
    }
}
