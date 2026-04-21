using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(CaseNumber))]
[NavigationItem("Case Management")]
[XafDisplayName("Applications")]
public class Application : AuditDetails
{
    private Guid _id;
    [VisibleInListView(false)]
    [VisibleInDetailView(false)]
    public virtual Guid Id
    {
        get => _id;
        set { if (_id != value) { RaisePropertyChanging(nameof(Id)); _id = value; RaisePropertyChanged(nameof(Id)); } }
    }

    private string _caseNumber = string.Empty;
    [XafDisplayName("Case Number")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string CaseNumber
    {
        get => _caseNumber;
        set { if (_caseNumber != value) { RaisePropertyChanging(nameof(CaseNumber)); _caseNumber = value; RaisePropertyChanged(nameof(CaseNumber)); } }
    }

    private ApplicationType _applicationType;
    [XafDisplayName("Application Type")]
    public virtual ApplicationType ApplicationType
    {
        get => _applicationType;
        set { if (_applicationType != value) { RaisePropertyChanging(nameof(ApplicationType)); _applicationType = value; RaisePropertyChanged(nameof(ApplicationType)); } }
    }

    private ApplicationStatus _status = ApplicationStatus.SUBMITTED;
    [XafDisplayName("Status")]
    public virtual ApplicationStatus Status
    {
        get => _status;
        set { if (_status != value) { RaisePropertyChanging(nameof(Status)); _status = value; RaisePropertyChanged(nameof(Status)); } }
    }

    // ── Assignment ────────────────────────────────────────────────────────────

    private string? _assignedToStaffId;
    [Browsable(false)]
    public virtual string? AssignedToStaffId
    {
        get => _assignedToStaffId;
        set { if (_assignedToStaffId != value) { RaisePropertyChanging(nameof(AssignedToStaffId)); _assignedToStaffId = value; RaisePropertyChanged(nameof(AssignedToStaffId)); } }
    }

    private string? _assignedToName;
    [XafDisplayName("Assigned To")]
    [VisibleInListView(true)]
    public virtual string? AssignedToName
    {
        get => _assignedToName;
        set { if (_assignedToName != value) { RaisePropertyChanging(nameof(AssignedToName)); _assignedToName = value; RaisePropertyChanged(nameof(AssignedToName)); } }
    }

    private DateTime? _assignedAt;
    [XafDisplayName("Assigned At")]
    [VisibleInListView(false)]
    public virtual DateTime? AssignedAt
    {
        get => _assignedAt;
        set { if (_assignedAt != value) { RaisePropertyChanging(nameof(AssignedAt)); _assignedAt = value; RaisePropertyChanged(nameof(AssignedAt)); } }
    }

    [Browsable(false)]
    public virtual string? AssignedByStaffId { get; set; }

    // ── Timestamps ────────────────────────────────────────────────────────────

    private DateTime _submittedAt = DateTime.UtcNow;
    [XafDisplayName("Submitted At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime SubmittedAt
    {
        get => _submittedAt;
        set { if (_submittedAt != value) { RaisePropertyChanging(nameof(SubmittedAt)); _submittedAt = value; RaisePropertyChanged(nameof(SubmittedAt)); } }
    }

    private DateTime _lastActivityAt = DateTime.UtcNow;
    [XafDisplayName("Last Activity")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime LastActivityAt
    {
        get => _lastActivityAt;
        set { if (_lastActivityAt != value) { RaisePropertyChanging(nameof(LastActivityAt)); _lastActivityAt = value; RaisePropertyChanged(nameof(LastActivityAt)); } }
    }

    // ── Submission metadata ───────────────────────────────────────────────────

    [VisibleInListView(false)]
    public virtual bool EmailVerified { get; set; } = false;

    [VisibleInListView(false)]
    public virtual bool CertificationAccepted { get; set; } = false;

    [XafDisplayName("Signature Name")]
    [VisibleInListView(false)]
    public virtual string? SignatureFullName { get; set; }

    [VisibleInListView(false)]
    public virtual DateTime? SignatureTimestamp { get; set; }

    [Browsable(false)]
    public virtual string? SubmissionIpAddress { get; set; }

    [Browsable(false)]
    public virtual string? SubmissionUserAgent { get; set; }

    // ── Narrative ─────────────────────────────────────────────────────────────

    private string? _narrativeStatement;
    [XafDisplayName("Narrative Statement")]
    [VisibleInListView(false)]
    public virtual string? NarrativeStatement
    {
        get => _narrativeStatement;
        set { if (_narrativeStatement != value) { RaisePropertyChanging(nameof(NarrativeStatement)); _narrativeStatement = value; RaisePropertyChanged(nameof(NarrativeStatement)); } }
    }

    [XafDisplayName("Prior Contact — Dealer")]
    [VisibleInListView(false)]
    public virtual bool? PriorContactDealer { get; set; }

    [XafDisplayName("Prior Contact — Manufacturer")]
    [VisibleInListView(false)]
    public virtual bool? PriorContactMfr { get; set; }

    [XafDisplayName("Prior Contact Notes")]
    [VisibleInListView(false)]
    public virtual string? PriorContactNotes { get; set; }

    [XafDisplayName("Desired Resolution")]
    [VisibleInListView(false)]
    public virtual DesiredResolution? DesiredResolution { get; set; }

    // ── Navigation properties ─────────────────────────────────────────────────

    [XafDisplayName("Applicant")]
    public virtual Applicant? Applicant { get; set; }

    [XafDisplayName("Vehicle")]
    public virtual Vehicle? Vehicle { get; set; }

    [Browsable(false)]
    public virtual ApplicationToken? Token { get; set; }

    [XafDisplayName("Defects")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Defect> Defects { get; set; } = new ObservableCollection<Defect>();

    [XafDisplayName("Repair Attempts")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<RepairAttempt> RepairAttempts { get; set; } = new ObservableCollection<RepairAttempt>();

    [XafDisplayName("Expenses")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Expense> Expenses { get; set; } = new ObservableCollection<Expense>();

    [XafDisplayName("Documents")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<ApplicationDocument> Documents { get; set; } = new ObservableCollection<ApplicationDocument>();

    [XafDisplayName("Case Timeline")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<CaseEvent> Events { get; set; } = new ObservableCollection<CaseEvent>();

    [XafDisplayName("Internal Notes")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<CaseNote> Notes { get; set; } = new ObservableCollection<CaseNote>();

    [XafDisplayName("Correspondence")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Correspondence> Correspondences { get; set; } = new ObservableCollection<Correspondence>();

    [XafDisplayName("Dealer Outreach")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<DealerOutreach> DealerOutreaches { get; set; } = new ObservableCollection<DealerOutreach>();

    [XafDisplayName("Hearings")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Hearing> Hearings { get; set; } = new ObservableCollection<Hearing>();

    [XafDisplayName("Decision")]
    public virtual Decision? Decision { get; set; }
}
