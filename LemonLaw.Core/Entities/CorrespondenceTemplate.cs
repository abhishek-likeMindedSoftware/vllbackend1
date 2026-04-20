using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(TemplateName))]
[NavigationItem("Administration")]
[XafDisplayName("Correspondence Templates")]
public class CorrespondenceTemplate : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual string TemplateCode { get; set; } = string.Empty;
    public virtual string TemplateName { get; set; } = string.Empty;
    public virtual string Subject { get; set; } = string.Empty;
    public virtual string BodyHtml { get; set; } = string.Empty;
    public virtual string BodyText { get; set; } = string.Empty;
    public virtual string? MergeFields { get; set; }
    public virtual bool IsActive { get; set; } = true;
    public virtual DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;
    public virtual string LastModifiedByName { get; set; } = string.Empty;
}
