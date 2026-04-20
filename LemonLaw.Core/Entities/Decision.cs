using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DecisionType))]
[NavigationItem("Case Management")]
[XafDisplayName("Decision")]
public class Decision : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private DecisionType _decisionType;
    [XafDisplayName("Decision Type")]
    public virtual DecisionType DecisionType
    {
        get => _decisionType;
        set { if (_decisionType != value) { RaisePropertyChanging(nameof(DecisionType)); _decisionType = value; RaisePropertyChanged(nameof(DecisionType)); } }
    }

    [XafDisplayName("Decision Date")]
    public virtual DateOnly DecisionDate { get; set; }

    [Browsable(false)]
    public virtual Guid? DecisionDocumentId { get; set; }

    [XafDisplayName("Refund Amount ($)")]
    [VisibleInListView(false)]
    public virtual decimal? RefundAmount { get; set; }

    [XafDisplayName("Compliance Deadline")]
    [VisibleInListView(false)]
    public virtual DateOnly? ComplianceDeadline { get; set; }

    [Browsable(false)]
    public virtual string DecisionIssuedById { get; set; } = string.Empty;

    [XafDisplayName("Consumer Notified")]
    [VisibleInListView(false)]
    public virtual bool ConsumerNotified { get; set; } = false;

    [XafDisplayName("Dealer Notified")]
    [VisibleInListView(false)]
    public virtual bool DealerNotified { get; set; } = false;
}
