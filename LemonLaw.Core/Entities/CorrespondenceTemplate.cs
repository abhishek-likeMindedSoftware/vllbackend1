using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(TemplateName))]
[NavigationItem("Administration")]
[XafDisplayName("Correspondence Template")]
public class CorrespondenceTemplate : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    private string _templateCode = string.Empty;
    [XafDisplayName("Template Code")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string TemplateCode
    {
        get => _templateCode;
        set { if (_templateCode != value) { RaisePropertyChanging(nameof(TemplateCode)); _templateCode = value; RaisePropertyChanged(nameof(TemplateCode)); } }
    }

    private string _templateName = string.Empty;
    [XafDisplayName("Template Name")]
    public virtual string TemplateName
    {
        get => _templateName;
        set { if (_templateName != value) { RaisePropertyChanging(nameof(TemplateName)); _templateName = value; RaisePropertyChanged(nameof(TemplateName)); } }
    }

    [XafDisplayName("Subject")]
    public virtual string Subject { get; set; } = string.Empty;

    [XafDisplayName("HTML Body")]
    [VisibleInListView(false)]
    public virtual string BodyHtml { get; set; } = string.Empty;

    [XafDisplayName("Plain Text Body")]
    [VisibleInListView(false)]
    public virtual string BodyText { get; set; } = string.Empty;

    [XafDisplayName("Merge Fields")]
    [VisibleInListView(false)]
    public virtual string? MergeFields { get; set; }

    private bool _isActive = true;
    [XafDisplayName("Active")]
    public virtual bool IsActive
    {
        get => _isActive;
        set { if (_isActive != value) { RaisePropertyChanging(nameof(IsActive)); _isActive = value; RaisePropertyChanged(nameof(IsActive)); } }
    }

    [XafDisplayName("Last Modified")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [VisibleInListView(false)]
    public virtual DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    [XafDisplayName("Last Modified By")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [VisibleInListView(false)]
    public virtual string LastModifiedByName { get; set; } = string.Empty;
}
