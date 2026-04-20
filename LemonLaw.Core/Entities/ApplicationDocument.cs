using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class ApplicationDocument : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual DocumentType DocumentType { get; set; }
    public virtual string FileName { get; set; } = string.Empty;
    public virtual string StoragePath { get; set; } = string.Empty;
    public virtual long FileSizeBytes { get; set; }
    public virtual string MimeType { get; set; } = string.Empty;
    public virtual DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public virtual UploadedByRole UploadedByRole { get; set; }
    public virtual DocumentStatus Status { get; set; } = DocumentStatus.PENDING_REVIEW;
    public virtual string? StaffNotes { get; set; }
    public virtual VirusScanResult VirusScanResult { get; set; } = VirusScanResult.PENDING;
    public virtual bool IsVisible_Consumer { get; set; } = true;
    public virtual bool IsVisible_Dealer { get; set; } = false;
}
