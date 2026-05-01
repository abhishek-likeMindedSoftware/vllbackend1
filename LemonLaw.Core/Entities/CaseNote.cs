using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(NoteText))]
[XafDisplayName("Case Note")]
public class CaseNote : AuditDetails,
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

    private Application? _application;

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-Notes")]
    public virtual Application? Application
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
    // showing "Note - <CaseNumber>" instead of the DefaultProperty (NoteText).
    [NotMapped, Browsable(false)]
    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public string Caption => $"Note - {Application?.CaseNumber ?? Id.ToString()[..8]}";

    #region Note Details

    private string _noteText = string.Empty;

    [XafDisplayName("Note")]
    public virtual string NoteText
    {
        get => _noteText;
        set
        {
            if (_noteText != value)
            {
                RaisePropertyChanging(nameof(NoteText));
                _noteText = value;
                RaisePropertyChanged(nameof(NoteText));
            }
        }
    }

    private string _createdByStaffId = string.Empty;

    [Browsable(false)]
    public virtual string CreatedByStaffId
    {
        get => _createdByStaffId;
        set
        {
            if (_createdByStaffId != value)
            {
                RaisePropertyChanging(nameof(CreatedByStaffId));
                _createdByStaffId = value;
                RaisePropertyChanged(nameof(CreatedByStaffId));
            }
        }
    }

    private string _createdByName = string.Empty;

    [XafDisplayName("Created By")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string CreatedByName
    {
        get => _createdByName;
        set
        {
            if (_createdByName != value)
            {
                RaisePropertyChanging(nameof(CreatedByName));
                _createdByName = value;
                RaisePropertyChanged(nameof(CreatedByName));
            }
        }
    }

    private DateTime _createdAt = DateTime.UtcNow;

    [XafDisplayName("Created At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime CreatedAt
    {
        get => _createdAt;
        set
        {
            if (_createdAt != value)
            {
                RaisePropertyChanging(nameof(CreatedAt));
                _createdAt = value;
                RaisePropertyChanged(nameof(CreatedAt));
            }
        }
    }

    private bool _isPinned;

    [XafDisplayName("Pinned")]
    public virtual bool IsPinned
    {
        get => _isPinned;
        set
        {
            if (_isPinned != value)
            {
                RaisePropertyChanging(nameof(IsPinned));
                _isPinned = value;
                RaisePropertyChanged(nameof(IsPinned));
            }
        }
    }

    #endregion
}
