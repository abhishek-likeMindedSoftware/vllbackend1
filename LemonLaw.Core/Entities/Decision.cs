using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Decision : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual DecisionType DecisionType { get; set; }
    public virtual DateOnly DecisionDate { get; set; }
    public virtual Guid? DecisionDocumentId { get; set; }
    public virtual decimal? RefundAmount { get; set; }
    public virtual DateOnly? ComplianceDeadline { get; set; }
    public virtual string DecisionIssuedById { get; set; } = string.Empty;
    public virtual bool ConsumerNotified { get; set; } = false;
    public virtual bool DealerNotified { get; set; } = false;
}
