using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(HearingDate))]
[NavigationItem("Hearings")]
[XafDisplayName("Hearing")]
public class Hearing : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private DateTime _hearingDate;
    [XafDisplayName("Hearing Date")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime HearingDate
    {
        get => _hearingDate;
        set { if (_hearingDate != value) { RaisePropertyChanging(nameof(HearingDate)); _hearingDate = value; RaisePropertyChanged(nameof(HearingDate)); } }
    }

    [XafDisplayName("Format")]
    public virtual HearingFormat HearingFormat { get; set; }

    [XafDisplayName("Location / URL")]
    [VisibleInListView(false)]
    public virtual string? HearingLocation { get; set; }

    [XafDisplayName("Arbitrator")]
    [VisibleInListView(false)]
    public virtual string? ArbitratorName { get; set; }

    [XafDisplayName("Consumer Notified")]
    [VisibleInListView(false)]
    public virtual bool ConsumerNoticesSent { get; set; } = false;

    [XafDisplayName("Dealer Notified")]
    [VisibleInListView(false)]
    public virtual bool DealerNoticesSent { get; set; } = false;

    private HearingOutcome _outcome = HearingOutcome.PENDING;
    [XafDisplayName("Outcome")]
    public virtual HearingOutcome Outcome
    {
        get => _outcome;
        set { if (_outcome != value) { RaisePropertyChanging(nameof(Outcome)); _outcome = value; RaisePropertyChanged(nameof(Outcome)); } }
    }

    [XafDisplayName("Outcome Notes")]
    [VisibleInListView(false)]
    public virtual string? OutcomeNotes { get; set; }

    [XafDisplayName("Continued To")]
    [VisibleInListView(false)]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? ContinuedTo { get; set; }
}
