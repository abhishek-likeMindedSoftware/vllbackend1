using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class DealerResponse : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid OutreachId { get; set; }
    public virtual DealerOutreach? Outreach { get; set; }

    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string ResponderName { get; set; } = string.Empty;
    public virtual string? ResponderTitle { get; set; }
    public virtual string ResponderEmail { get; set; } = string.Empty;
    public virtual string? ResponderPhone { get; set; }
    public virtual DealerPosition DealerPosition { get; set; }
    public virtual string ResponseNarrative { get; set; } = string.Empty;
    public virtual string? RepairHistoryNotes { get; set; }
    public virtual decimal? SettlementOffer { get; set; }
    public virtual string? SettlementDetails { get; set; }
    public virtual bool CertificationAccepted { get; set; }
    public virtual string CertifierFullName { get; set; } = string.Empty;
    public virtual DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public virtual string SubmissionIpAddress { get; set; } = string.Empty;
}
