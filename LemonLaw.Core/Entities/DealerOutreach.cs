using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DealerName))]
[NavigationItem("Dealer Portal Activity")]
[XafDisplayName("Dealer Outreach")]
public class DealerOutreach : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private string _dealerName = string.Empty;
    [XafDisplayName("Dealer Name")]
    public virtual string DealerName
    {
        get => _dealerName;
        set { if (_dealerName != value) { RaisePropertyChanging(nameof(DealerName)); _dealerName = value; RaisePropertyChanged(nameof(DealerName)); } }
    }

    [XafDisplayName("Dealer Email")]
    public virtual string DealerEmail { get; set; } = string.Empty;

    [XafDisplayName("Dealer Phone")]
    [VisibleInListView(false)]
    public virtual string? DealerPhone { get; set; }

    [XafDisplayName("Outreach Type")]
    public virtual OutreachType OutreachType { get; set; }

    [XafDisplayName("Sent At")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? SentAt { get; set; }

    [Browsable(false)]
    public virtual string SentByStaffId { get; set; } = string.Empty;

    [Browsable(false)]
    public virtual string TemplateUsed { get; set; } = string.Empty;

    [Browsable(false)]
    public virtual string TokenHash { get; set; } = string.Empty;

    [Browsable(false)]
    public virtual DateTime TokenCreatedAt { get; set; } = DateTime.UtcNow;

    [Browsable(false)]
    public virtual DateTime TokenExpiresAt { get; set; }

    [XafDisplayName("Response Deadline")]
    public virtual DateOnly ResponseDeadline { get; set; }

    [Browsable(false)]
    public virtual string? SendGridMessageId { get; set; }

    [XafDisplayName("Delivery Status")]
    [VisibleInListView(false)]
    public virtual EmailDeliveryStatus DeliveryStatus { get; set; } = EmailDeliveryStatus.PENDING;

    private OutreachStatus _status = OutreachStatus.PENDING;
    [XafDisplayName("Status")]
    public virtual OutreachStatus Status
    {
        get => _status;
        set { if (_status != value) { RaisePropertyChanging(nameof(Status)); _status = value; RaisePropertyChanged(nameof(Status)); } }
    }

    // ── Follow-up tracking ────────────────────────────────────────────────────

    [XafDisplayName("Follow-Up 1 Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FollowUp1SentAt { get; set; }

    [XafDisplayName("Follow-Up 2 Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FollowUp2SentAt { get; set; }

    [XafDisplayName("Final Notice Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FinalNoticeSentAt { get; set; }

    [XafDisplayName("Escalated to Hearing")]
    [VisibleInListView(false)]
    public virtual bool EscalatedToHearing { get; set; } = false;

    [XafDisplayName("Escalation Notes")]
    [VisibleInListView(false)]
    public virtual string? EscalationNotes { get; set; }

    [XafDisplayName("Response")]
    public virtual DealerResponse? Response { get; set; }
}
