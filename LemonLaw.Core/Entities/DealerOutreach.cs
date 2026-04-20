using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DealerName))]
[NavigationItem("Dealer Portal Activity")]
[XafDisplayName("Dealer Outreach")]
public class DealerOutreach : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string DealerName { get; set; } = string.Empty;
    public virtual string DealerEmail { get; set; } = string.Empty;
    public virtual string? DealerPhone { get; set; }
    public virtual OutreachType OutreachType { get; set; }
    public virtual DateTime? SentAt { get; set; }
    public virtual string SentByStaffId { get; set; } = string.Empty;
    public virtual string TemplateUsed { get; set; } = string.Empty;
    public virtual string TokenHash { get; set; } = string.Empty;
    public virtual DateTime TokenCreatedAt { get; set; } = DateTime.UtcNow;
    public virtual DateTime TokenExpiresAt { get; set; }
    public virtual DateOnly ResponseDeadline { get; set; }
    public virtual string? SendGridMessageId { get; set; }
    public virtual EmailDeliveryStatus DeliveryStatus { get; set; } = EmailDeliveryStatus.PENDING;
    public virtual OutreachStatus Status { get; set; } = OutreachStatus.PENDING;

    // Follow-up tracking
    public virtual DateTime? FollowUp1SentAt { get; set; }
    public virtual DateTime? FollowUp2SentAt { get; set; }
    public virtual DateTime? FinalNoticeSentAt { get; set; }
    public virtual bool EscalatedToHearing { get; set; } = false;
    public virtual string? EscalationNotes { get; set; }

    public virtual DealerResponse? Response { get; set; }
}
