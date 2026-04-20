using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(HearingDate))]
[NavigationItem("Hearings")]
[XafDisplayName("Hearings")]
public class Hearing : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual DateTime HearingDate { get; set; }
    public virtual HearingFormat HearingFormat { get; set; }
    public virtual string? HearingLocation { get; set; }
    public virtual string? ArbitratorName { get; set; }
    public virtual bool ConsumerNoticesSent { get; set; } = false;
    public virtual bool DealerNoticesSent { get; set; } = false;
    public virtual HearingOutcome Outcome { get; set; } = HearingOutcome.PENDING;
    public virtual string? OutcomeNotes { get; set; }
    public virtual DateTime? ContinuedTo { get; set; }
}
