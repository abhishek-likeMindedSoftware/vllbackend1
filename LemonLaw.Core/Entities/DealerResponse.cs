using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(ResponderName))]
[XafDisplayName("Dealer Response")]
public class DealerResponse : AuditDetails,
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

    #region Outreach Relationship (M:1)

    private Guid? _outreachId;

    [Browsable(false)]
    public virtual Guid? OutreachId
    {
        get => _outreachId;
        set
        {
            if (_outreachId != value)
            {
                RaisePropertyChanging(nameof(OutreachId));
                _outreachId = value;
                RaisePropertyChanged(nameof(OutreachId));
            }
        }
    }

    private DealerOutreach? _outreach;

    [ForeignKey("OutreachId")]
    [DevExpress.Xpo.Association("DealerOutreach-Response")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual DealerOutreach? Outreach
    {
        get => _outreach;
        set
        {
            if (_outreach != value)
            {
                RaisePropertyChanging(nameof(Outreach));
                _outreach = value;
                OutreachId = value?.Id;
                RaisePropertyChanged(nameof(Outreach));
            }
        }
    }

    #endregion

    #region Application Relationship (M:1)

    private Guid? _applicationId;

    [Browsable(false)]
    public virtual Guid? ApplicationId
    {
        get => _applicationId;
        set
        {
            if (_applicationId != value)
            {
                RaisePropertyChanging(nameof(ApplicationId));
                _applicationId = value;
                RaisePropertyChanged(nameof(ApplicationId));
            }
        }
    }

    private VllApplication? _application;

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-DealerResponses")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual VllApplication? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }

    #endregion

    #region Response Details

    private string _responderName = string.Empty;

    [XafDisplayName("Responder Name")]
    public virtual string ResponderName
    {
        get => _responderName;
        set
        {
            if (_responderName != value)
            {
                RaisePropertyChanging(nameof(ResponderName));
                _responderName = value;
                RaisePropertyChanged(nameof(ResponderName));
            }
        }
    }

    private string? _responderTitle;

    [XafDisplayName("Title")]
    [VisibleInListView(false)]
    public virtual string? ResponderTitle
    {
        get => _responderTitle;
        set
        {
            if (_responderTitle != value)
            {
                RaisePropertyChanging(nameof(ResponderTitle));
                _responderTitle = value;
                RaisePropertyChanged(nameof(ResponderTitle));
            }
        }
    }

    private string _responderEmail = string.Empty;

    [XafDisplayName("Responder Email")]
    public virtual string ResponderEmail
    {
        get => _responderEmail;
        set
        {
            if (_responderEmail != value)
            {
                RaisePropertyChanging(nameof(ResponderEmail));
                _responderEmail = value;
                RaisePropertyChanged(nameof(ResponderEmail));
            }
        }
    }

    private string? _responderPhone;

    [XafDisplayName("Responder Phone")]
    [VisibleInListView(false)]
    public virtual string? ResponderPhone
    {
        get => _responderPhone;
        set
        {
            if (_responderPhone != value)
            {
                RaisePropertyChanging(nameof(ResponderPhone));
                _responderPhone = value;
                RaisePropertyChanged(nameof(ResponderPhone));
            }
        }
    }

    private DealerPosition _dealerPosition;

    [XafDisplayName("Dealer Position")]
    public virtual DealerPosition DealerPosition
    {
        get => _dealerPosition;
        set
        {
            if (_dealerPosition != value)
            {
                RaisePropertyChanging(nameof(DealerPosition));
                _dealerPosition = value;
                RaisePropertyChanged(nameof(DealerPosition));
            }
        }
    }

    private string _responseNarrative = string.Empty;

    [XafDisplayName("Response Narrative")]
    [VisibleInListView(false)]
    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
    public virtual string ResponseNarrative
    {
        get => _responseNarrative;
        set
        {
            if (_responseNarrative != value)
            {
                RaisePropertyChanging(nameof(ResponseNarrative));
                _responseNarrative = value;
                RaisePropertyChanged(nameof(ResponseNarrative));
            }
        }
    }

    private string? _repairHistoryNotes;

    [XafDisplayName("Repair History Notes")]
    [VisibleInListView(false)]
    [EditorAlias(EditorAliases.RichTextPropertyEditor)]
    public virtual string? RepairHistoryNotes
    {
        get => _repairHistoryNotes;
        set
        {
            if (_repairHistoryNotes != value)
            {
                RaisePropertyChanging(nameof(RepairHistoryNotes));
                _repairHistoryNotes = value;
                RaisePropertyChanged(nameof(RepairHistoryNotes));
            }
        }
    }

    private decimal? _settlementOffer;

    [XafDisplayName("Settlement Offer ($)")]
    [VisibleInListView(false)]
    public virtual decimal? SettlementOffer
    {
        get => _settlementOffer;
        set
        {
            if (_settlementOffer != value)
            {
                RaisePropertyChanging(nameof(SettlementOffer));
                _settlementOffer = value;
                RaisePropertyChanged(nameof(SettlementOffer));
            }
        }
    }

    private string? _settlementDetails;

    [XafDisplayName("Settlement Details")]
    [VisibleInListView(false)]
    public virtual string? SettlementDetails
    {
        get => _settlementDetails;
        set
        {
            if (_settlementDetails != value)
            {
                RaisePropertyChanging(nameof(SettlementDetails));
                _settlementDetails = value;
                RaisePropertyChanged(nameof(SettlementDetails));
            }
        }
    }

    private bool _certificationAccepted;

    [XafDisplayName("Certified")]
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

    private string _certifierFullName = string.Empty;

    [XafDisplayName("Certifier Name")]
    [VisibleInListView(false)]
    public virtual string CertifierFullName
    {
        get => _certifierFullName;
        set
        {
            if (_certifierFullName != value)
            {
                RaisePropertyChanging(nameof(CertifierFullName));
                _certifierFullName = value;
                RaisePropertyChanged(nameof(CertifierFullName));
            }
        }
    }

    private DateTime _submittedAt = DateTime.UtcNow;

    [XafDisplayName("Submitted At")]
    [ReadOnly(true)]
    [ModelDefault("AllowEdit", "False")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
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

    private string _submissionIpAddress = string.Empty;

    [Browsable(false)]
    public virtual string SubmissionIpAddress
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

    #endregion
}
