using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(NoteText))]
[XafDisplayName("Case Note")]
public class CaseNote : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private string _noteText = string.Empty;
    [XafDisplayName("Note")]
    public virtual string NoteText
    {
        get => _noteText;
        set { if (_noteText != value) { RaisePropertyChanging(nameof(NoteText)); _noteText = value; RaisePropertyChanged(nameof(NoteText)); } }
    }

    [Browsable(false)]
    public virtual string CreatedByStaffId { get; set; } = string.Empty;

    [XafDisplayName("Created By")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string CreatedByName { get; set; } = string.Empty;

    [XafDisplayName("Created At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private bool _isPinned;
    [XafDisplayName("Pinned")]
    public virtual bool IsPinned
    {
        get => _isPinned;
        set { if (_isPinned != value) { RaisePropertyChanging(nameof(IsPinned)); _isPinned = value; RaisePropertyChanged(nameof(IsPinned)); } }
    }
}
