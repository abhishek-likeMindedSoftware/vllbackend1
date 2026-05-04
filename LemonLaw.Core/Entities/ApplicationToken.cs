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

[XafDisplayName("Application Token")]
public class ApplicationToken : INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
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

    public ApplicationToken() { }

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

    private VllApplication? _application;

    [ForeignKey(nameof(ApplicationId))]
    [Browsable(false)]
    public virtual VllApplication? Application
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

    #region Token Details

    private TokenType _tokenType;

    [Browsable(false)]
    public virtual TokenType TokenType
    {
        get => _tokenType;
        set
        {
            if (_tokenType != value)
            {
                RaisePropertyChanging(nameof(TokenType));
                _tokenType = value;
                RaisePropertyChanged(nameof(TokenType));
            }
        }
    }

    private string _tokenHash = string.Empty;

    [Browsable(false)]
    public virtual string TokenHash
    {
        get => _tokenHash;
        set
        {
            if (_tokenHash != value)
            {
                RaisePropertyChanging(nameof(TokenHash));
                _tokenHash = value;
                RaisePropertyChanged(nameof(TokenHash));
            }
        }
    }

    private DateTime _expiresAt;

    [XafDisplayName("Expires At")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
    [VisibleInListView(false)]
    public virtual DateTime ExpiresAt
    {
        get => _expiresAt;
        set
        {
            if (_expiresAt != value)
            {
                RaisePropertyChanging(nameof(ExpiresAt));
                _expiresAt = value;
                RaisePropertyChanged(nameof(ExpiresAt));
            }
        }
    }

    private DateTime? _lastUsedAt;

    [XafDisplayName("Last Used")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
    [VisibleInListView(false)]
    public virtual DateTime? LastUsedAt
    {
        get => _lastUsedAt;
        set
        {
            if (_lastUsedAt != value)
            {
                RaisePropertyChanging(nameof(LastUsedAt));
                _lastUsedAt = value;
                RaisePropertyChanged(nameof(LastUsedAt));
            }
        }
    }

    private bool _isRevoked;

    [XafDisplayName("Revoked")]
    [VisibleInListView(false)]
    public virtual bool IsRevoked
    {
        get => _isRevoked;
        set
        {
            if (_isRevoked != value)
            {
                RaisePropertyChanging(nameof(IsRevoked));
                _isRevoked = value;
                RaisePropertyChanged(nameof(IsRevoked));
            }
        }
    }

    private DateTime? _revokedAt;

    [VisibleInListView(false)]
    public virtual DateTime? RevokedAt
    {
        get => _revokedAt;
        set
        {
            if (_revokedAt != value)
            {
                RaisePropertyChanging(nameof(RevokedAt));
                _revokedAt = value;
                RaisePropertyChanged(nameof(RevokedAt));
            }
        }
    }

    private string? _revokedByStaffId;

    [Browsable(false)]
    public virtual string? RevokedByStaffId
    {
        get => _revokedByStaffId;
        set
        {
            if (_revokedByStaffId != value)
            {
                RaisePropertyChanging(nameof(RevokedByStaffId));
                _revokedByStaffId = value;
                RaisePropertyChanged(nameof(RevokedByStaffId));
            }
        }
    }

    #endregion
}
