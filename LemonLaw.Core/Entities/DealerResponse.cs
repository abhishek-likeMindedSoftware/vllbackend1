using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(ResponderName))]
[XafDisplayName("Dealer Response")]
public class DealerResponse : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid OutreachId { get; set; }

    [Browsable(false)]
    public virtual DealerOutreach? Outreach { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private string _responderName = string.Empty;
    [XafDisplayName("Responder Name")]
    public virtual string ResponderName
    {
        get => _responderName;
        set { if (_responderName != value) { RaisePropertyChanging(nameof(ResponderName)); _responderName = value; RaisePropertyChanged(nameof(ResponderName)); } }
    }

    [XafDisplayName("Title")]
    [VisibleInListView(false)]
    public virtual string? ResponderTitle { get; set; }

    [XafDisplayName("Responder Email")]
    public virtual string ResponderEmail { get; set; } = string.Empty;

    [XafDisplayName("Responder Phone")]
    [VisibleInListView(false)]
    public virtual string? ResponderPhone { get; set; }

    private DealerPosition _dealerPosition;
    [XafDisplayName("Dealer Position")]
    public virtual DealerPosition DealerPosition
    {
        get => _dealerPosition;
        set { if (_dealerPosition != value) { RaisePropertyChanging(nameof(DealerPosition)); _dealerPosition = value; RaisePropertyChanged(nameof(DealerPosition)); } }
    }

    [XafDisplayName("Response Narrative")]
    [VisibleInListView(false)]
    public virtual string ResponseNarrative { get; set; } = string.Empty;

    [XafDisplayName("Repair History Notes")]
    [VisibleInListView(false)]
    public virtual string? RepairHistoryNotes { get; set; }

    [XafDisplayName("Settlement Offer ($)")]
    [VisibleInListView(false)]
    public virtual decimal? SettlementOffer { get; set; }

    [XafDisplayName("Settlement Details")]
    [VisibleInListView(false)]
    public virtual string? SettlementDetails { get; set; }

    [XafDisplayName("Certified")]
    [VisibleInListView(false)]
    public virtual bool CertificationAccepted { get; set; }

    [XafDisplayName("Certifier Name")]
    [VisibleInListView(false)]
    public virtual string CertifierFullName { get; set; } = string.Empty;

    [XafDisplayName("Submitted At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [Browsable(false)]
    public virtual string SubmissionIpAddress { get; set; } = string.Empty;
}
