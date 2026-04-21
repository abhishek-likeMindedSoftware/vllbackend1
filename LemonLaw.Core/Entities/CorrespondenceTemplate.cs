using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(TemplateName))]
[NavigationItem("Administration")]
[XafDisplayName("Correspondence Template")]
public class CorrespondenceTemplate : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF
    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void RaisePropertyChanged(string propertyName)
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

    public CorrespondenceTemplate() { }

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

    #region Template Details

    private string _templateCode = string.Empty;

    [XafDisplayName("Template Code")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string TemplateCode
    {
        get => _templateCode;
        set
        {
            if (_templateCode != value)
            {
                RaisePropertyChanging(nameof(TemplateCode));
                _templateCode = value;
                RaisePropertyChanged(nameof(TemplateCode));
            }
        }
    }

    private string _templateName = string.Empty;

    [XafDisplayName("Template Name")]
    public virtual string TemplateName
    {
        get => _templateName;
        set
        {
            if (_templateName != value)
            {
                RaisePropertyChanging(nameof(TemplateName));
                _templateName = value;
                RaisePropertyChanged(nameof(TemplateName));
            }
        }
    }

    private string _subject = string.Empty;

    [XafDisplayName("Subject")]
    public virtual string Subject
    {
        get => _subject;
        set
        {
            if (_subject != value)
            {
                RaisePropertyChanging(nameof(Subject));
                _subject = value;
                RaisePropertyChanged(nameof(Subject));
            }
        }
    }

    private string _bodyHtml = string.Empty;

    [XafDisplayName("HTML Body")]
    [VisibleInListView(false)]
    public virtual string BodyHtml
    {
        get => _bodyHtml;
        set
        {
            if (_bodyHtml != value)
            {
                RaisePropertyChanging(nameof(BodyHtml));
                _bodyHtml = value;
                RaisePropertyChanged(nameof(BodyHtml));
            }
        }
    }

    private string _bodyText = string.Empty;

    [XafDisplayName("Plain Text Body")]
    [VisibleInListView(false)]
    public virtual string BodyText
    {
        get => _bodyText;
        set
        {
            if (_bodyText != value)
            {
                RaisePropertyChanging(nameof(BodyText));
                _bodyText = value;
                RaisePropertyChanged(nameof(BodyText));
            }
        }
    }

    private string? _mergeFields;

    [XafDisplayName("Merge Fields")]
    [VisibleInListView(false)]
    public virtual string? MergeFields
    {
        get => _mergeFields;
        set
        {
            if (_mergeFields != value)
            {
                RaisePropertyChanging(nameof(MergeFields));
                _mergeFields = value;
                RaisePropertyChanged(nameof(MergeFields));
            }
        }
    }

    private bool _isActive = true;

    [XafDisplayName("Active")]
    public virtual bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                RaisePropertyChanging(nameof(IsActive));
                _isActive = value;
                RaisePropertyChanged(nameof(IsActive));
            }
        }
    }

    private DateTime _lastModifiedAt = DateTime.UtcNow;

    [XafDisplayName("Last Modified")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [VisibleInListView(false)]
    public virtual DateTime LastModifiedAt
    {
        get => _lastModifiedAt;
        set
        {
            if (_lastModifiedAt != value)
            {
                RaisePropertyChanging(nameof(LastModifiedAt));
                _lastModifiedAt = value;
                RaisePropertyChanged(nameof(LastModifiedAt));
            }
        }
    }

    private string _lastModifiedByName = string.Empty;

    [XafDisplayName("Last Modified By")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [VisibleInListView(false)]
    public virtual string LastModifiedByName
    {
        get => _lastModifiedByName;
        set
        {
            if (_lastModifiedByName != value)
            {
                RaisePropertyChanging(nameof(LastModifiedByName));
                _lastModifiedByName = value;
                RaisePropertyChanged(nameof(LastModifiedByName));
            }
        }
    }

    #endregion
}
