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

[DefaultProperty(nameof(DealerName))]
[NavigationItem("Dealer Portal Activity")]
[XafDisplayName("Dealer Outreach")]
public class DealerOutreach : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
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

    public DealerOutreach() { }

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

    #region Application Relationship (M:1)
    private Guid _applicationId;

    [Browsable(false)]
    public virtual Guid ApplicationId
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

    private Application? _application;

    [ForeignKey(nameof(ApplicationId))]
    [Browsable(false)]
    public virtual Application? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id ?? Guid.Empty;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }
    #endregion

    #region Outreach Details

    private string _dealerName = string.Empty;

    [XafDisplayName("Dealer Name")]
    public virtual string DealerName
    {
        get => _dealerName;
        set
        {
            if (_dealerName != value)
            {
                RaisePropertyChanging(nameof(DealerName));
                _dealerName = value;
                RaisePropertyChanged(nameof(DealerName));
            }
        }
    }

    private string _dealerEmail = string.Empty;

    [XafDisplayName("Dealer Email")]
    public virtual string DealerEmail
    {
        get => _dealerEmail;
        set
        {
            if (_dealerEmail != value)
            {
                RaisePropertyChanging(nameof(DealerEmail));
                _dealerEmail = value;
                RaisePropertyChanged(nameof(DealerEmail));
            }
        }
    }

    private string? _dealerPhone;

    [XafDisplayName("Dealer Phone")]
    [VisibleInListView(false)]
    public virtual string? DealerPhone
    {
        get => _dealerPhone;
        set
        {
            if (_dealerPhone != value)
            {
                RaisePropertyChanging(nameof(DealerPhone));
                _dealerPhone = value;
                RaisePropertyChanged(nameof(DealerPhone));
            }
        }
    }

    private OutreachType _outreachType;

    [XafDisplayName("Outreach Type")]
    public virtual OutreachType OutreachType
    {
        get => _outreachType;
        set
        {
            if (_outreachType != value)
            {
                RaisePropertyChanging(nameof(OutreachType));
                _outreachType = value;
                RaisePropertyChanged(nameof(OutreachType));
            }
        }
    }

    private DateTime? _sentAt;

    [XafDisplayName("Sent At")]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? SentAt
    {
        get => _sentAt;
        set
        {
            if (_sentAt != value)
            {
                RaisePropertyChanging(nameof(SentAt));
                _sentAt = value;
                RaisePropertyChanged(nameof(SentAt));
            }
        }
    }

    private string _sentByStaffId = string.Empty;

    [Browsable(false)]
    public virtual string SentByStaffId
    {
        get => _sentByStaffId;
        set
        {
            if (_sentByStaffId != value)
            {
                RaisePropertyChanging(nameof(SentByStaffId));
                _sentByStaffId = value;
                RaisePropertyChanged(nameof(SentByStaffId));
            }
        }
    }

    private string _templateUsed = string.Empty;

    [Browsable(false)]
    public virtual string TemplateUsed
    {
        get => _templateUsed;
        set
        {
            if (_templateUsed != value)
            {
                RaisePropertyChanging(nameof(TemplateUsed));
                _templateUsed = value;
                RaisePropertyChanged(nameof(TemplateUsed));
            }
        }
    }

    private string _tokenHash = string.Empty;

    [Browsable(false)]
    public virtual string TokenHash
    {
        get => _tokenHash;
        set
        {
            if (_tokenHash != value)
            {
                RaisePropertyChanging(nameof(TokenHash));
                _tokenHash = value;
                RaisePropertyChanged(nameof(TokenHash));
            }
        }
    }

    private DateTime _tokenCreatedAt = DateTime.UtcNow;

    [Browsable(false)]
    public virtual DateTime TokenCreatedAt
    {
        get => _tokenCreatedAt;
        set
        {
            if (_tokenCreatedAt != value)
            {
                RaisePropertyChanging(nameof(TokenCreatedAt));
                _tokenCreatedAt = value;
                RaisePropertyChanged(nameof(TokenCreatedAt));
            }
        }
    }

    private DateTime _tokenExpiresAt;

    [Browsable(false)]
    public virtual DateTime TokenExpiresAt
    {
        get => _tokenExpiresAt;
        set
        {
            if (_tokenExpiresAt != value)
            {
                RaisePropertyChanging(nameof(TokenExpiresAt));
                _tokenExpiresAt = value;
                RaisePropertyChanged(nameof(TokenExpiresAt));
            }
        }
    }

    private DateOnly _responseDeadline;

    [XafDisplayName("Response Deadline")]
    public virtual DateOnly ResponseDeadline
    {
        get => _responseDeadline;
        set
        {
            if (_responseDeadline != value)
            {
                RaisePropertyChanging(nameof(ResponseDeadline));
                _responseDeadline = value;
                RaisePropertyChanged(nameof(ResponseDeadline));
            }
        }
    }

    private string? _sendGridMessageId;

    [Browsable(false)]
    public virtual string? SendGridMessageId
    {
        get => _sendGridMessageId;
        set
        {
            if (_sendGridMessageId != value)
            {
                RaisePropertyChanging(nameof(SendGridMessageId));
                _sendGridMessageId = value;
                RaisePropertyChanged(nameof(SendGridMessageId));
            }
        }
    }

    private EmailDeliveryStatus _deliveryStatus = EmailDeliveryStatus.PENDING;

    [XafDisplayName("Delivery Status")]
    [VisibleInListView(false)]
    public virtual EmailDeliveryStatus DeliveryStatus
    {
        get => _deliveryStatus;
        set
        {
            if (_deliveryStatus != value)
            {
                RaisePropertyChanging(nameof(DeliveryStatus));
                _deliveryStatus = value;
                RaisePropertyChanged(nameof(DeliveryStatus));
            }
        }
    }

    private OutreachStatus _status = OutreachStatus.PENDING;

    [XafDisplayName("Status")]
    public virtual OutreachStatus Status
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

    #region Follow-Up Tracking

    private DateTime? _followUp1SentAt;

    [XafDisplayName("Follow-Up 1 Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FollowUp1SentAt
    {
        get => _followUp1SentAt;
        set
        {
            if (_followUp1SentAt != value)
            {
                RaisePropertyChanging(nameof(FollowUp1SentAt));
                _followUp1SentAt = value;
                RaisePropertyChanged(nameof(FollowUp1SentAt));
            }
        }
    }

    private DateTime? _followUp2SentAt;

    [XafDisplayName("Follow-Up 2 Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FollowUp2SentAt
    {
        get => _followUp2SentAt;
        set
        {
            if (_followUp2SentAt != value)
            {
                RaisePropertyChanging(nameof(FollowUp2SentAt));
                _followUp2SentAt = value;
                RaisePropertyChanged(nameof(FollowUp2SentAt));
            }
        }
    }

    private DateTime? _finalNoticeSentAt;

    [XafDisplayName("Final Notice Sent")]
    [VisibleInListView(false)]
    public virtual DateTime? FinalNoticeSentAt
    {
        get => _finalNoticeSentAt;
        set
        {
            if (_finalNoticeSentAt != value)
            {
                RaisePropertyChanging(nameof(FinalNoticeSentAt));
                _finalNoticeSentAt = value;
                RaisePropertyChanged(nameof(FinalNoticeSentAt));
            }
        }
    }

    private bool _escalatedToHearing;

    [XafDisplayName("Escalated to Hearing")]
    [VisibleInListView(false)]
    public virtual bool EscalatedToHearing
    {
        get => _escalatedToHearing;
        set
        {
            if (_escalatedToHearing != value)
            {
                RaisePropertyChanging(nameof(EscalatedToHearing));
                _escalatedToHearing = value;
                RaisePropertyChanged(nameof(EscalatedToHearing));
            }
        }
    }

    private string? _escalationNotes;

    [XafDisplayName("Escalation Notes")]
    [VisibleInListView(false)]
    public virtual string? EscalationNotes
    {
        get => _escalationNotes;
        set
        {
            if (_escalationNotes != value)
            {
                RaisePropertyChanging(nameof(EscalationNotes));
                _escalationNotes = value;
                RaisePropertyChanged(nameof(EscalationNotes));
            }
        }
    }

    #endregion

    #region Response Relationship (1:1)
    private Guid? _responseId;

    [Browsable(false)]
    public virtual Guid? ResponseId
    {
        get => _responseId;
        set
        {
            if (_responseId != value)
            {
                RaisePropertyChanging(nameof(ResponseId));
                _responseId = value;
                RaisePropertyChanged(nameof(ResponseId));
            }
        }
    }

    private DealerResponse? _response;

    [XafDisplayName("Response")]
    public virtual DealerResponse? Response
    {
        get => _response;
        set
        {
            if (_response != value)
            {
                RaisePropertyChanging(nameof(Response));
                _response = value;
                ResponseId = value?.Id;
                RaisePropertyChanged(nameof(Response));
            }
        }
    }
    #endregion
}
