using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[XafDisplayName("Application Token")]
public class ApplicationToken : INotifyPropertyChanging, INotifyPropertyChanged
{
    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanging(string p) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(p));
    protected void RaisePropertyChanged(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    [Browsable(false)]
    public virtual TokenType TokenType { get; set; }

    [Browsable(false)]
    public virtual string TokenHash { get; set; } = string.Empty;

    [XafDisplayName("Expires At")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [VisibleInListView(false)]
    public virtual DateTime ExpiresAt { get; set; }

    [XafDisplayName("Last Used")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [VisibleInListView(false)]
    public virtual DateTime? LastUsedAt { get; set; }

    private bool _isRevoked;
    [XafDisplayName("Revoked")]
    [VisibleInListView(false)]
    public virtual bool IsRevoked
    {
        get => _isRevoked;
        set { if (_isRevoked != value) { RaisePropertyChanging(nameof(IsRevoked)); _isRevoked = value; RaisePropertyChanged(nameof(IsRevoked)); } }
    }

    [VisibleInListView(false)]
    public virtual DateTime? RevokedAt { get; set; }

    [Browsable(false)]
    public virtual string? RevokedByStaffId { get; set; }
}
