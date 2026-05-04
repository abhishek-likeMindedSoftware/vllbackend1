using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(FileName))]
[XafDisplayName("Document")]
public class ApplicationDocument : AuditDetails,
        INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF & INotify
    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanging(string propertyName) =>
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

    protected void RaisePropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                RaisePropertyChanging(nameof(ObjectSpace));
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    private Guid _id = Guid.NewGuid();

    [Key]
    [XafDisplayName("ID")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual Guid Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                RaisePropertyChanging(nameof(Id));
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }
    }

    #region Application Relationship (M:1)

    private Guid? _applicationId;

    [Browsable(false)]
    public virtual Guid? ApplicationId
    {
        get => _applicationId;
        set
        {
            if (_applicationId != value)
            {
                RaisePropertyChanging(nameof(ApplicationId));
                _applicationId = value;
                RaisePropertyChanged(nameof(ApplicationId));
            }
        }
    }

    private VllApplication? _application;

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-Documents")]
    public virtual VllApplication? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }

    #endregion

    // Caption is referenced by ObjectCaptionFormat="{0:Caption}" in Model.DesignedDiffs.xafml.
    // XAF uses it to set the tab/window title when this record is opened in a detail view,
    // showing "Document - <CaseNumber>" instead of the DefaultProperty (FileName).
    [NotMapped, Browsable(false)]
    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public string Caption => $"Document - {Application?.CaseNumber ?? Id.ToString()[..8]}";

    #region Document Details

    private DocumentType _documentType;

    [XafDisplayName("Document Type")]
    public virtual DocumentType DocumentType
    {
        get => _documentType;
        set
        {
            if (_documentType != value)
            {
                RaisePropertyChanging(nameof(DocumentType));
                _documentType = value;
                RaisePropertyChanged(nameof(DocumentType));
            }
        }
    }

    private string _fileName = string.Empty;

    [XafDisplayName("File Name")]
    public virtual string FileName
    {
        get => _fileName;
        set
        {
            if (_fileName != value)
            {
                RaisePropertyChanging(nameof(FileName));
                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }
    }

    private string _storagePath = string.Empty;

    [Browsable(false)]
    public virtual string StoragePath
    {
        get => _storagePath;
        set
        {
            if (_storagePath != value)
            {
                RaisePropertyChanging(nameof(StoragePath));
                _storagePath = value;
                RaisePropertyChanged(nameof(StoragePath));
            }
        }
    }

    private long _fileSizeBytes;

    [XafDisplayName("File Size (bytes)")]
    [VisibleInListView(false)]
    public virtual long FileSizeBytes
    {
        get => _fileSizeBytes;
        set
        {
            if (_fileSizeBytes != value)
            {
                RaisePropertyChanging(nameof(FileSizeBytes));
                _fileSizeBytes = value;
                RaisePropertyChanged(nameof(FileSizeBytes));
            }
        }
    }

    private string _mimeType = string.Empty;

    [XafDisplayName("MIME Type")]
    [VisibleInListView(false)]
    public virtual string MimeType
    {
        get => _mimeType;
        set
        {
            if (_mimeType != value)
            {
                RaisePropertyChanging(nameof(MimeType));
                _mimeType = value;
                RaisePropertyChanged(nameof(MimeType));
            }
        }
    }

    private DateTime _uploadedAt = DateTime.UtcNow;

    [XafDisplayName("Uploaded At")]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
    public virtual DateTime UploadedAt
    {
        get => _uploadedAt;
        set
        {
            if (_uploadedAt != value)
            {
                RaisePropertyChanging(nameof(UploadedAt));
                _uploadedAt = value;
                RaisePropertyChanged(nameof(UploadedAt));
            }
        }
    }

    private UploadedByRole _uploadedByRole;

    [XafDisplayName("Uploaded By")]
    public virtual UploadedByRole UploadedByRole
    {
        get => _uploadedByRole;
        set
        {
            if (_uploadedByRole != value)
            {
                RaisePropertyChanging(nameof(UploadedByRole));
                _uploadedByRole = value;
                RaisePropertyChanged(nameof(UploadedByRole));
            }
        }
    }

    private DocumentStatus _status = DocumentStatus.PENDING_REVIEW;

    [XafDisplayName("Status")]
    public virtual DocumentStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                RaisePropertyChanging(nameof(Status));
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }
    }

    private string? _staffNotes;

    [XafDisplayName("Staff Notes")]
    [VisibleInListView(false)]
    public virtual string? StaffNotes
    {
        get => _staffNotes;
        set
        {
            if (_staffNotes != value)
            {
                RaisePropertyChanging(nameof(StaffNotes));
                _staffNotes = value;
                RaisePropertyChanged(nameof(StaffNotes));
            }
        }
    }

    private VirusScanResult _virusScanResult = VirusScanResult.PENDING;

    [XafDisplayName("Virus Scan")]
    [VisibleInListView(false)]
    public virtual VirusScanResult VirusScanResult
    {
        get => _virusScanResult;
        set
        {
            if (_virusScanResult != value)
            {
                RaisePropertyChanging(nameof(VirusScanResult));
                _virusScanResult = value;
                RaisePropertyChanged(nameof(VirusScanResult));
            }
        }
    }

    private bool _isVisibleConsumer = true;

    [XafDisplayName("Visible to Consumer")]
    [VisibleInListView(false)]
    public virtual bool IsVisible_Consumer
    {
        get => _isVisibleConsumer;
        set
        {
            if (_isVisibleConsumer != value)
            {
                RaisePropertyChanging(nameof(IsVisible_Consumer));
                _isVisibleConsumer = value;
                RaisePropertyChanged(nameof(IsVisible_Consumer));
            }
        }
    }

    private bool _isVisibleDealer;

    [XafDisplayName("Visible to Dealer")]
    [VisibleInListView(false)]
    public virtual bool IsVisible_Dealer
    {
        get => _isVisibleDealer;
        set
        {
            if (_isVisibleDealer != value)
            {
                RaisePropertyChanging(nameof(IsVisible_Dealer));
                _isVisibleDealer = value;
                RaisePropertyChanged(nameof(IsVisible_Dealer));
            }
        }
    }

    #endregion
}
