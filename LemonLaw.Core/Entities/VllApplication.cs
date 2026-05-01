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
public class VllApplication : AuditDetails,
     INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF & INotify
    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanging(string propertyName) =>
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    protected void RaisePropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                RaisePropertyChanging(nameof(ObjectSpace));
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    public VllApplication() { }

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
    [VisibleInDetailView(false)]
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
    [VisibleInDetailView(false)]
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
    [VisibleInDetailView(false)]
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
    [VisibleInListView(true), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInDetailView(false)]
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
    [VisibleInDetailView(false)]
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

    [VisibleInListView(false), VisibleInDetailView(false)]
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

    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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

    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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
    [VisibleInListView(false), VisibleInDetailView(false)]
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

    #region Defects Relationship (1:M)

    private readonly ObservableCollection<Defect> _defects = new();

    [XafDisplayName("Defects")]
    [DevExpress.Xpo.Association("Application-Defects")]
    [VisibleInDetailView(false)]
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
                {
                    foreach (var item in value)
                        _defects.Add(item);
                }
                RaisePropertyChanged(nameof(Defects));
            }
        }
    }

    #endregion

    #region Navigation Properties

    [XafDisplayName("Applicant")]
    [VisibleInDetailView(false)]
    public virtual Applicant? Applicant { get; set; }

    [XafDisplayName("Vehicle")]
    [VisibleInDetailView(false)]
    public virtual Vehicle? Vehicle { get; set; }

    [Browsable(false)]
    public virtual ApplicationToken? Token { get; set; }

    private readonly ObservableCollection<RepairAttempt> _repairAttempts = new();

    [XafDisplayName("Repair Attempts")]
    [DevExpress.Xpo.Association("Application-RepairAttempts")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Expenses")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Documents")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Events")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Notes")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Correspondences")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-DealerOutreaches")]
    [VisibleInDetailView(false)]
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
    [DevExpress.Xpo.Association("Application-Hearings")]
    [VisibleInDetailView(false)]
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
    [VisibleInDetailView(false)]
    public virtual Decision? Decision { get; set; }

    #endregion

    #region Custom Detail View Hook

    /// <summary>
    /// The only property visible in the Detail View.
    /// ApplicationDetailPropertyEditor renders the full custom Blazor component.
    /// </summary>
    [NotMapped]
    [XafDisplayName("Case Detail")]
    [VisibleInDetailView(true), VisibleInListView(false)]
    public object? DetailPanel => this;

    #endregion
}
