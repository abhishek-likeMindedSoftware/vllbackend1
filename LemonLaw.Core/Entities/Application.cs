using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(CaseNumber))]
[NavigationItem("Case Management")]
[XafDisplayName("Applications")]
public class Application : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual string CaseNumber { get; set; } = string.Empty;
    public virtual ApplicationType ApplicationType { get; set; }
    public virtual ApplicationStatus Status { get; set; } = ApplicationStatus.SUBMITTED;

    // Assignment
    public virtual string? AssignedToStaffId { get; set; }
    public virtual string? AssignedToName { get; set; }
    public virtual DateTime? AssignedAt { get; set; }
    public virtual string? AssignedByStaffId { get; set; }

    // Timestamps
    public virtual DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public virtual DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    // Submission
    public virtual bool EmailVerified { get; set; } = false;
    public virtual bool CertificationAccepted { get; set; } = false;
    public virtual string? SignatureFullName { get; set; }
    public virtual DateTime? SignatureTimestamp { get; set; }
    public virtual string? SubmissionIpAddress { get; set; }
    public virtual string? SubmissionUserAgent { get; set; }

    // Narrative
    public virtual string? NarrativeStatement { get; set; }
    public virtual bool? PriorContactDealer { get; set; }
    public virtual bool? PriorContactMfr { get; set; }
    public virtual string? PriorContactNotes { get; set; }
    public virtual DesiredResolution? DesiredResolution { get; set; }

    // Navigation
    public virtual Applicant? Applicant { get; set; }
    public virtual Vehicle? Vehicle { get; set; }
    public virtual ApplicationToken? Token { get; set; }
    public virtual ICollection<Defect> Defects { get; set; } = new List<Defect>();
    public virtual ICollection<RepairAttempt> RepairAttempts { get; set; } = new List<RepairAttempt>();
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public virtual ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
    public virtual ICollection<CaseEvent> Events { get; set; } = new List<CaseEvent>();
    public virtual ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();
    public virtual ICollection<Correspondence> Correspondences { get; set; } = new List<Correspondence>();
    public virtual ICollection<DealerOutreach> DealerOutreaches { get; set; } = new List<DealerOutreach>();
    public virtual ICollection<Hearing> Hearings { get; set; } = new List<Hearing>();
    public virtual Decision? Decision { get; set; }
}
