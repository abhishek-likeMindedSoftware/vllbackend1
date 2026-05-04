using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Application.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

/// <summary>
/// Service for generating hearing notice letters using Word templates.
/// Uses DevExpress RichEdit to load templates and replace placeholders.
/// </summary>
public class HearingLetterService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IGenericRepository<ApplicationDocument> _documentRepository;
    private readonly ILetterTemplateService _templateService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HearingLetterService> _logger;
    private readonly string _documentsBasePath;

    public HearingLetterService(
        IApplicationRepository applicationRepository,
        IGenericRepository<ApplicationDocument> documentRepository,
        ILetterTemplateService templateService,
        IConfiguration configuration,
        ILogger<HearingLetterService> logger)
    {
        _applicationRepository = applicationRepository;
        _documentRepository = documentRepository;
        _templateService = templateService;
        _configuration = configuration;
        _logger = logger;
        
        _documentsBasePath = _configuration["DocumentStorage:BasePath"] 
            ?? Path.Combine(Directory.GetCurrentDirectory(), "Documents");
        
        if (!Directory.Exists(_documentsBasePath))
            Directory.CreateDirectory(_documentsBasePath);
    }

    public async Task<ApplicationDocument?> GenerateHearingNoticeAsync(
        Guid applicationId,
        Guid hearingId,
        string staffId)
    {
        try
        {
            var application = await _applicationRepository.GetWithFullDetailsAsync(applicationId);

            if (application == null)
            {
                _logger.LogError("Application {ApplicationId} not found", applicationId);
                return null;
            }

            var hearing = application.Hearings.FirstOrDefault(h => h.Id == hearingId);
            if (hearing == null)
            {
                _logger.LogError("Hearing {HearingId} not found", hearingId);
                return null;
            }

            // Generate document from template
            var docxBytes = await _templateService.GenerateHearingNoticeAsync(application, hearing);

            var fileName = $"Hearing_Notice_{application.CaseNumber}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.docx";
            var filePath = Path.Combine(_documentsBasePath, fileName);

            // Save to disk
            await File.WriteAllBytesAsync(filePath, docxBytes);

            var fileInfo = new FileInfo(filePath);
            var document = new ApplicationDocument
            {
                ApplicationId = applicationId,
                DocumentType = DocumentType.LETTER,
                FileName = fileName,
                StoragePath = filePath,
                FileSizeBytes = fileInfo.Length,
                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                UploadedAt = DateTime.UtcNow,
                UploadedByRole = UploadedByRole.STAFF,
                Status = DocumentStatus.ACCEPTED,
                VirusScanResult = VirusScanResult.CLEAN,
                IsVisible_Consumer = true,
                IsVisible_Dealer = false,
                StaffNotes = $"Auto-generated hearing notice for hearing on {hearing.HearingDate:MM/dd/yyyy}"
            };

            await _documentRepository.AddAsync(document);
            await _documentRepository.SaveChangesAsync();

            _logger.LogInformation("Generated hearing notice Word document for application {ApplicationId}, hearing {HearingId}", 
                applicationId, hearingId);

            return document;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating hearing notice for application {ApplicationId}", applicationId);
            return null;
        }
    }
}
