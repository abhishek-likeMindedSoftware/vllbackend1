using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

/// <summary>
/// Append-only audit log entry. Does NOT inherit AuditDetails (no soft delete, no audit fields).
/// </summary>
[DefaultProperty(nameof(Description))]
[XafDisplayName("Case Event")]
public class CaseEvent : INotifyPropertyChanging, INotifyPropertyChanged
{
    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanging(string p) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(p));
    protected void RaisePropertyChanged(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid CaseEventId { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private CaseEventType _eventType;
    [XafDisplayName("Event Type")]
    public virtual CaseEventType EventType
    {
        get => _eventType;
        set { if (_eventType != value) { RaisePropertyChanging(nameof(EventType)); _eventType = value; RaisePropertyChanged(nameof(EventType)); } }
    }

    [XafDisplayName("Timestamp")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime EventTimestamp { get; set; } = DateTime.UtcNow;

    [XafDisplayName("Actor Type")]
    public virtual ActorType ActorType { get; set; }

    [Browsable(false)]
    public virtual string? ActorId { get; set; }

    [XafDisplayName("Actor")]
    public virtual string ActorDisplayName { get; set; } = string.Empty;

    private string _description = string.Empty;
    [XafDisplayName("Description")]
    public virtual string Description
    {
        get => _description;
        set { if (_description != value) { RaisePropertyChanging(nameof(Description)); _description = value; RaisePropertyChanged(nameof(Description)); } }
    }

    [XafDisplayName("Previous Value")]
    [VisibleInListView(false)]
    public virtual string? PreviousValue { get; set; }

    [XafDisplayName("New Value")]
    [VisibleInListView(false)]
    public virtual string? NewValue { get; set; }
}
