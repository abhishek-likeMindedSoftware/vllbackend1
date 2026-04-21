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

/// <summary>
/// Append-only audit log entry. Does NOT inherit AuditDetails (no soft delete, no audit fields).
/// </summary>
[DefaultProperty(nameof(Description))]
[XafDisplayName("Case Event")]
public class CaseEvent : INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF
    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanging(string propertyName)
        => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    protected void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    public CaseEvent() { }

    private Guid _caseEventId = Guid.NewGuid();

    [Key]
    [XafDisplayName("ID")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual Guid CaseEventId
    {
        get => _caseEventId;
        set
        {
            if (_caseEventId != value)
            {
                RaisePropertyChanging(nameof(CaseEventId));
                _caseEventId = value;
                RaisePropertyChanged(nameof(CaseEventId));
            }
        }
    }

    #region Application Relationship (M:1)
    private Guid _applicationId;

    [Browsable(false)]
    public virtual Guid ApplicationId
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

    [ForeignKey(nameof(ApplicationId))]
    [Browsable(false)]
    public virtual Application? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id ?? Guid.Empty;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }
    #endregion

    #region Event Details

    private CaseEventType _eventType;

    [XafDisplayName("Event Type")]
    public virtual CaseEventType EventType
    {
        get => _eventType;
        set
        {
            if (_eventType != value)
            {
                RaisePropertyChanging(nameof(EventType));
                _eventType = value;
                RaisePropertyChanged(nameof(EventType));
            }
        }
    }

    private DateTime _eventTimestamp = DateTime.UtcNow;

    [XafDisplayName("Timestamp")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime EventTimestamp
    {
        get => _eventTimestamp;
        set
        {
            if (_eventTimestamp != value)
            {
                RaisePropertyChanging(nameof(EventTimestamp));
                _eventTimestamp = value;
                RaisePropertyChanged(nameof(EventTimestamp));
            }
        }
    }

    private ActorType _actorType;

    [XafDisplayName("Actor Type")]
    public virtual ActorType ActorType
    {
        get => _actorType;
        set
        {
            if (_actorType != value)
            {
                RaisePropertyChanging(nameof(ActorType));
                _actorType = value;
                RaisePropertyChanged(nameof(ActorType));
            }
        }
    }

    private string? _actorId;

    [Browsable(false)]
    public virtual string? ActorId
    {
        get => _actorId;
        set
        {
            if (_actorId != value)
            {
                RaisePropertyChanging(nameof(ActorId));
                _actorId = value;
                RaisePropertyChanged(nameof(ActorId));
            }
        }
    }

    private string _actorDisplayName = string.Empty;

    [XafDisplayName("Actor")]
    public virtual string ActorDisplayName
    {
        get => _actorDisplayName;
        set
        {
            if (_actorDisplayName != value)
            {
                RaisePropertyChanging(nameof(ActorDisplayName));
                _actorDisplayName = value;
                RaisePropertyChanged(nameof(ActorDisplayName));
            }
        }
    }

    private string _description = string.Empty;

    [XafDisplayName("Description")]
    public virtual string Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                RaisePropertyChanging(nameof(Description));
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
    }

    private string? _previousValue;

    [XafDisplayName("Previous Value")]
    [VisibleInListView(false)]
    public virtual string? PreviousValue
    {
        get => _previousValue;
        set
        {
            if (_previousValue != value)
            {
                RaisePropertyChanging(nameof(PreviousValue));
                _previousValue = value;
                RaisePropertyChanged(nameof(PreviousValue));
            }
        }
    }

    private string? _newValue;

    [XafDisplayName("New Value")]
    [VisibleInListView(false)]
    public virtual string? NewValue
    {
        get => _newValue;
        set
        {
            if (_newValue != value)
            {
                RaisePropertyChanging(nameof(NewValue));
                _newValue = value;
                RaisePropertyChanged(nameof(NewValue));
            }
        }
    }

    #endregion
}
