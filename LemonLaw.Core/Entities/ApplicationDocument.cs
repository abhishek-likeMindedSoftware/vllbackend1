using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(FileName))]
[XafDisplayName("Document")]
public class ApplicationDocument : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    [XafDisplayName("Document Type")]
    public virtual DocumentType DocumentType { get; set; }

    private string _fileName = string.Empty;
    [XafDisplayName("File Name")]
    public virtual string FileName
    {
        get => _fileName;
        set { if (_fileName != value) { RaisePropertyChanging(nameof(FileName)); _fileName = value; RaisePropertyChanged(nameof(FileName)); } }
    }

    [Browsable(false)]
    public virtual string StoragePath { get; set; } = string.Empty;

    [XafDisplayName("File Size (bytes)")]
    [VisibleInListView(false)]
    public virtual long FileSizeBytes { get; set; }

    [XafDisplayName("MIME Type")]
    [VisibleInListView(false)]
    public virtual string MimeType { get; set; } = string.Empty;

    [XafDisplayName("Uploaded At")]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [XafDisplayName("Uploaded By")]
    public virtual UploadedByRole UploadedByRole { get; set; }

    private DocumentStatus _status = DocumentStatus.PENDING_REVIEW;
    [XafDisplayName("Status")]
    public virtual DocumentStatus Status
    {
        get => _status;
        set { if (_status != value) { RaisePropertyChanging(nameof(Status)); _status = value; RaisePropertyChanged(nameof(Status)); } }
    }

    [XafDisplayName("Staff Notes")]
    [VisibleInListView(false)]
    public virtual string? StaffNotes { get; set; }

    [XafDisplayName("Virus Scan")]
    [VisibleInListView(false)]
    public virtual VirusScanResult VirusScanResult { get; set; } = VirusScanResult.PENDING;

    [XafDisplayName("Visible to Consumer")]
    [VisibleInListView(false)]
    public virtual bool IsVisible_Consumer { get; set; } = true;

    [XafDisplayName("Visible to Dealer")]
    [VisibleInListView(false)]
    public virtual bool IsVisible_Dealer { get; set; } = false;
}
