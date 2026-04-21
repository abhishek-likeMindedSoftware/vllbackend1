using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(CaseNumber))]
[NavigationItem("Case Management")]
[XafDisplayName("Applications")]
public class Application : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF
    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    public Application() { }

    private Guid _id = Guid.NewGuid();

    [Key]
    [XafDisplayName("ID")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual Guid Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                RaisePropertyChanging(nameof(Id));
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }
    }

    #region Core Fields

    private string _caseNumber = string.Empty;

    [XafDisplayName("Case Number")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    public virtual string CaseNumber
    {
        get => _caseNumber;
        set
        {
            if (_caseNumber != value)
            {
                RaisePropertyChanging(nameof(CaseNumber));
                _caseNumber = value;
                RaisePropertyChanged(nameof(CaseNumber));
            }
        }
    }

    private ApplicationType _applicationType;

    [XafDisplayName("Application Type")]
    public virtual ApplicationType ApplicationType
    {
        get => _applicationType;
        set
        {
            if (_applicationType != value)
            {
                RaisePropertyChanging(nameof(ApplicationType));
                _applicationType = value;
                RaisePropertyChanged(nameof(ApplicationType));
            }
        }
    }

    private ApplicationStatus _status = ApplicationStatus.SUBMITTED;

    [XafDisplayName("Status")]
    public virtual ApplicationStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                RaisePropertyChanging(nameof(Status));
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }
    }

    #endregion

    #region Assignment

    private string? _assignedToStaffId;

    [Browsable(false)]
    public virtual string? AssignedToStaffId
    {
        get => _assignedToStaffId;
        set
        {
            if (_assignedToStaffId != value)
            {
                RaisePropertyChanging(nameof(AssignedToStaffId));
                _assignedToStaffId = value;
                RaisePropertyChanged(nameof(AssignedToStaffId));
            }
        }
    }

    private string? _assignedToName;

    [XafDisplayName("Assigned To")]
    [VisibleInListView(true)]
    public virtual string? AssignedToName
    {
        get => _assignedToName;
        set
        {
            if (_assignedToName != value)
            {
                RaisePropertyChanging(nameof(AssignedToName));
                _assignedToName = value;
                RaisePropertyChanged(nameof(AssignedToName));
            }
        }
    }

    private DateTime? _assignedAt;

    [XafDisplayName("Assigned At")]
    [VisibleInListView(false)]
    public virtual DateTime? AssignedAt
    {
        get => _assignedAt;
        set
        {
            if (_assignedAt != value)
            {
                RaisePropertyChanging(nameof(AssignedAt));
                _assignedAt = value;
                RaisePropertyChanged(nameof(AssignedAt));
            }
        }
    }

    private string? _assignedByStaffId;

    [Browsable(false)]
    public virtual string? AssignedByStaffId
    {
        get => _assignedByStaffId;
        set
        {
            if (_assignedByStaffId != value)
            {
                RaisePropertyChanging(nameof(AssignedByStaffId));
                _assignedByStaffId = value;
                RaisePropertyChanged(nameof(AssignedByStaffId));
            }
        }
    }

    #endregion

    #region Timestamps

    private DateTime _submittedAt = DateTime.UtcNow;

    [XafDisplayName("Submitted At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime SubmittedAt
    {
        get => _submittedAt;
        set
        {
            if (_submittedAt != value)
            {
                RaisePropertyChanging(nameof(SubmittedAt));
                _submittedAt = value;
                RaisePropertyChanged(nameof(SubmittedAt));
            }
        }
    }

    private DateTime _lastActivityAt = DateTime.UtcNow;

    [XafDisplayName("Last Activity")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime LastActivityAt
    {
        get => _lastActivityAt;
        set
        {
            if (_lastActivityAt != value)
            {
                RaisePropertyChanging(nameof(LastActivityAt));
                _lastActivityAt = value;
                RaisePropertyChanged(nameof(LastActivityAt));
            }
        }
    }

    #endregion

    #region Submission Metadata

    private bool _emailVerified;

    [VisibleInListView(false)]
    public virtual bool EmailVerified
    {
        get => _emailVerified;
        set
        {
            if (_emailVerified != value)
            {
                RaisePropertyChanging(nameof(EmailVerified));
                _emailVerified = value;
                RaisePropertyChanged(nameof(EmailVerified));
            }
        }
    }

    private bool _certificationAccepted;

    [VisibleInListView(false)]
    public virtual bool CertificationAccepted
    {
        get => _certificationAccepted;
        set
        {
            if (_certificationAccepted != value)
            {
                RaisePropertyChanging(nameof(CertificationAccepted));
                _certificationAccepted = value;
                RaisePropertyChanged(nameof(CertificationAccepted));
            }
        }
    }

    private string? _signatureFullName;

    [XafDisplayName("Signature Name")]
    [VisibleInListView(false)]
    public virtual string? SignatureFullName
    {
        get => _signatureFullName;
        set
        {
            if (_signatureFullName != value)
            {
                RaisePropertyChanging(nameof(SignatureFullName));
                _signatureFullName = value;
                RaisePropertyChanged(nameof(SignatureFullName));
            }
        }
    }

    private DateTime? _signatureTimestamp;

    [VisibleInListView(false)]
    public virtual DateTime? SignatureTimestamp
    {
        get => _signatureTimestamp;
        set
        {
            if (_signatureTimestamp != value)
            {
                RaisePropertyChanging(nameof(SignatureTimestamp));
                _signatureTimestamp = value;
                RaisePropertyChanged(nameof(SignatureTimestamp));
            }
        }
    }

    private string? _submissionIpAddress;

    [Browsable(false)]
    public virtual string? SubmissionIpAddress
    {
        get => _submissionIpAddress;
        set
        {
            if (_submissionIpAddress != value)
            {
                RaisePropertyChanging(nameof(SubmissionIpAddress));
                _submissionIpAddress = value;
                RaisePropertyChanged(nameof(SubmissionIpAddress));
            }
        }
    }

    private string? _submissionUserAgent;

    [Browsable(false)]
    public virtual string? SubmissionUserAgent
    {
        get => _submissionUserAgent;
        set
        {
            if (_submissionUserAgent != value)
            {
                RaisePropertyChanging(nameof(SubmissionUserAgent));
                _submissionUserAgent = value;
                RaisePropertyChanged(nameof(SubmissionUserAgent));
            }
        }
    }

    #endregion

    #region Narrative

    private string? _narrativeStatement;

    [XafDisplayName("Narrative Statement")]
    [VisibleInListView(false)]
    public virtual string? NarrativeStatement
    {
        get => _narrativeStatement;
        set
        {
            if (_narrativeStatement != value)
            {
                RaisePropertyChanging(nameof(NarrativeStatement));
                _narrativeStatement = value;
                RaisePropertyChanged(nameof(NarrativeStatement));
            }
        }
    }

    private bool? _priorContactDealer;

    [XafDisplayName("Prior Contact — Dealer")]
    [VisibleInListView(false)]
    public virtual bool? PriorContactDealer
    {
        get => _priorContactDealer;
        set
        {
            if (_priorContactDealer != value)
            {
                RaisePropertyChanging(nameof(PriorContactDealer));
                _priorContactDealer = value;
                RaisePropertyChanged(nameof(PriorContactDealer));
            }
        }
    }

    private bool? _priorContactMfr;

    [XafDisplayName("Prior Contact — Manufacturer")]
    [VisibleInListView(false)]
    public virtual bool? PriorContactMfr
    {
        get => _priorContactMfr;
        set
        {
            if (_priorContactMfr != value)
            {
                RaisePropertyChanging(nameof(PriorContactMfr));
                _priorContactMfr = value;
                RaisePropertyChanged(nameof(PriorContactMfr));
            }
        }
    }

    private string? _priorContactNotes;

    [XafDisplayName("Prior Contact Notes")]
    [VisibleInListView(false)]
    public virtual string? PriorContactNotes
    {
        get => _priorContactNotes;
        set
        {
            if (_priorContactNotes != value)
            {
                RaisePropertyChanging(nameof(PriorContactNotes));
                _priorContactNotes = value;
                RaisePropertyChanged(nameof(PriorContactNotes));
            }
        }
    }

    private DesiredResolution? _desiredResolution;

    [XafDisplayName("Desired Resolution")]
    [VisibleInListView(false)]
    public virtual DesiredResolution? DesiredResolution
    {
        get => _desiredResolution;
        set
        {
            if (_desiredResolution != value)
            {
                RaisePropertyChanging(nameof(DesiredResolution));
                _desiredResolution = value;
                RaisePropertyChanged(nameof(DesiredResolution));
            }
        }
    }

    #endregion

    #region Navigation Properties

    [XafDisplayName("Applicant")]
    public virtual Applicant? Applicant { get; set; }

    [XafDisplayName("Vehicle")]
    public virtual Vehicle? Vehicle { get; set; }

    [Browsable(false)]
    public virtual ApplicationToken? Token { get; set; }

    private readonly ObservableCollection<Defect> _defects = new();

    [XafDisplayName("Defects")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Defect> Defects
    {
        get => _defects;
        set
        {
            if (!ReferenceEquals(_defects, value))
            {
                RaisePropertyChanging(nameof(Defects));
                _defects.Clear();
                if (value != null)
                    foreach (var item in value) _defects.Add(item);
                RaisePropertyChanged(nameof(Defects));
            }
        }
    }

    private readonly ObservableCollection<RepairAttempt> _repairAttempts = new();

    [XafDisplayName("Repair Attempts")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<RepairAttempt> RepairAttempts
    {
        get => _repairAttempts;
        set
        {
            if (!ReferenceEquals(_repairAttempts, value))
            {
                RaisePropertyChanging(nameof(RepairAttempts));
                _repairAttempts.Clear();
                if (value != null)
                    foreach (var item in value) _repairAttempts.Add(item);
                RaisePropertyChanged(nameof(RepairAttempts));
            }
        }
    }

    private readonly ObservableCollection<Expense> _expenses = new();

    [XafDisplayName("Expenses")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Expense> Expenses
    {
        get => _expenses;
        set
        {
            if (!ReferenceEquals(_expenses, value))
            {
                RaisePropertyChanging(nameof(Expenses));
                _expenses.Clear();
                if (value != null)
                    foreach (var item in value) _expenses.Add(item);
                RaisePropertyChanged(nameof(Expenses));
            }
        }
    }

    private readonly ObservableCollection<ApplicationDocument> _documents = new();

    [XafDisplayName("Documents")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<ApplicationDocument> Documents
    {
        get => _documents;
        set
        {
            if (!ReferenceEquals(_documents, value))
            {
                RaisePropertyChanging(nameof(Documents));
                _documents.Clear();
                if (value != null)
                    foreach (var item in value) _documents.Add(item);
                RaisePropertyChanged(nameof(Documents));
            }
        }
    }

    private readonly ObservableCollection<CaseEvent> _events = new();

    [XafDisplayName("Case Timeline")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<CaseEvent> Events
    {
        get => _events;
        set
        {
            if (!ReferenceEquals(_events, value))
            {
                RaisePropertyChanging(nameof(Events));
                _events.Clear();
                if (value != null)
                    foreach (var item in value) _events.Add(item);
                RaisePropertyChanged(nameof(Events));
            }
        }
    }

    private readonly ObservableCollection<CaseNote> _notes = new();

    [XafDisplayName("Internal Notes")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<CaseNote> Notes
    {
        get => _notes;
        set
        {
            if (!ReferenceEquals(_notes, value))
            {
                RaisePropertyChanging(nameof(Notes));
                _notes.Clear();
                if (value != null)
                    foreach (var item in value) _notes.Add(item);
                RaisePropertyChanged(nameof(Notes));
            }
        }
    }

    private readonly ObservableCollection<Correspondence> _correspondences = new();

    [XafDisplayName("Correspondence")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Correspondence> Correspondences
    {
        get => _correspondences;
        set
        {
            if (!ReferenceEquals(_correspondences, value))
            {
                RaisePropertyChanging(nameof(Correspondences));
                _correspondences.Clear();
                if (value != null)
                    foreach (var item in value) _correspondences.Add(item);
                RaisePropertyChanged(nameof(Correspondences));
            }
        }
    }

    private readonly ObservableCollection<DealerOutreach> _dealerOutreaches = new();

    [XafDisplayName("Dealer Outreach")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<DealerOutreach> DealerOutreaches
    {
        get => _dealerOutreaches;
        set
        {
            if (!ReferenceEquals(_dealerOutreaches, value))
            {
                RaisePropertyChanging(nameof(DealerOutreaches));
                _dealerOutreaches.Clear();
                if (value != null)
                    foreach (var item in value) _dealerOutreaches.Add(item);
                RaisePropertyChanged(nameof(DealerOutreaches));
            }
        }
    }

    private readonly ObservableCollection<Hearing> _hearings = new();

    [XafDisplayName("Hearings")]
    [DevExpress.ExpressApp.DC.Aggregated]
    public virtual ICollection<Hearing> Hearings
    {
        get => _hearings;
        set
        {
            if (!ReferenceEquals(_hearings, value))
            {
                RaisePropertyChanging(nameof(Hearings));
                _hearings.Clear();
                if (value != null)
                    foreach (var item in value) _hearings.Add(item);
                RaisePropertyChanged(nameof(Hearings));
            }
        }
    }

    [XafDisplayName("Decision")]
    public virtual Decision? Decision { get; set; }

    #endregion
}
