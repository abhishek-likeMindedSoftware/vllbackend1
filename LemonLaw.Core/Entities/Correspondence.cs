using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(Subject))]
[XafDisplayName("Correspondence")]
public class Correspondence : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    [XafDisplayName("Direction")]
    public virtual CorrespondenceDirection Direction { get; set; }

    [XafDisplayName("Recipient Type")]
    public virtual CorrespondenceRecipientType RecipientType { get; set; }

    [XafDisplayName("Recipient Email")]
    public virtual string RecipientEmail { get; set; } = string.Empty;

    private string _subject = string.Empty;
    [XafDisplayName("Subject")]
    public virtual string Subject
    {
        get => _subject;
        set { if (_subject != value) { RaisePropertyChanging(nameof(Subject)); _subject = value; RaisePropertyChanged(nameof(Subject)); } }
    }

    [XafDisplayName("Preview")]
    [VisibleInListView(false)]
    public virtual string? BodyPreview { get; set; }

    [XafDisplayName("Template Used")]
    [VisibleInListView(false)]
    public virtual string? TemplateUsed { get; set; }

    [XafDisplayName("Sent At")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? SentAt { get; set; }

    [Browsable(false)]
    public virtual string? SentByStaffId { get; set; }

    [Browsable(false)]
    public virtual string? SendGridMessageId { get; set; }

    private EmailDeliveryStatus _deliveryStatus = EmailDeliveryStatus.PENDING;
    [XafDisplayName("Delivery Status")]
    public virtual EmailDeliveryStatus DeliveryStatus
    {
        get => _deliveryStatus;
        set { if (_deliveryStatus != value) { RaisePropertyChanging(nameof(DeliveryStatus)); _deliveryStatus = value; RaisePropertyChanged(nameof(DeliveryStatus)); } }
    }

    [XafDisplayName("Delivery Updated")]
    [VisibleInListView(false)]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? DeliveryUpdatedAt { get; set; }
}
