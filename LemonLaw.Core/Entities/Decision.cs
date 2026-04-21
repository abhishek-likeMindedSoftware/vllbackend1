using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DecisionType))]
[NavigationItem("Case Management")]
[XafDisplayName("Decision")]
public class Decision : AuditDetails,
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

    private Application? _application;

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-Decisions")]
    public virtual Application? Application
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

    #region Decision Details

    private DecisionType _decisionType;

    [XafDisplayName("Decision Type")]
    public virtual DecisionType DecisionType
    {
        get => _decisionType;
        set
        {
            if (_decisionType != value)
            {
                RaisePropertyChanging(nameof(DecisionType));
                _decisionType = value;
                RaisePropertyChanged(nameof(DecisionType));
            }
        }
    }

    private DateOnly _decisionDate;

    [XafDisplayName("Decision Date")]
    public virtual DateOnly DecisionDate
    {
        get => _decisionDate;
        set
        {
            if (_decisionDate != value)
            {
                RaisePropertyChanging(nameof(DecisionDate));
                _decisionDate = value;
                RaisePropertyChanged(nameof(DecisionDate));
            }
        }
    }

    private Guid? _decisionDocumentId;

    [Browsable(false)]
    public virtual Guid? DecisionDocumentId
    {
        get => _decisionDocumentId;
        set
        {
            if (_decisionDocumentId != value)
            {
                RaisePropertyChanging(nameof(DecisionDocumentId));
                _decisionDocumentId = value;
                RaisePropertyChanged(nameof(DecisionDocumentId));
            }
        }
    }

    private decimal? _refundAmount;

    [XafDisplayName("Refund Amount ($)")]
    [VisibleInListView(false)]
    public virtual decimal? RefundAmount
    {
        get => _refundAmount;
        set
        {
            if (_refundAmount != value)
            {
                RaisePropertyChanging(nameof(RefundAmount));
                _refundAmount = value;
                RaisePropertyChanged(nameof(RefundAmount));
            }
        }
    }

    private DateOnly? _complianceDeadline;

    [XafDisplayName("Compliance Deadline")]
    [VisibleInListView(false)]
    public virtual DateOnly? ComplianceDeadline
    {
        get => _complianceDeadline;
        set
        {
            if (_complianceDeadline != value)
            {
                RaisePropertyChanging(nameof(ComplianceDeadline));
                _complianceDeadline = value;
                RaisePropertyChanged(nameof(ComplianceDeadline));
            }
        }
    }

    private string _decisionIssuedById = string.Empty;

    [Browsable(false)]
    public virtual string DecisionIssuedById
    {
        get => _decisionIssuedById;
        set
        {
            if (_decisionIssuedById != value)
            {
                RaisePropertyChanging(nameof(DecisionIssuedById));
                _decisionIssuedById = value;
                RaisePropertyChanged(nameof(DecisionIssuedById));
            }
        }
    }

    private bool _consumerNotified;

    [XafDisplayName("Consumer Notified")]
    [VisibleInListView(false)]
    public virtual bool ConsumerNotified
    {
        get => _consumerNotified;
        set
        {
            if (_consumerNotified != value)
            {
                RaisePropertyChanging(nameof(ConsumerNotified));
                _consumerNotified = value;
                RaisePropertyChanged(nameof(ConsumerNotified));
            }
        }
    }

    private bool _dealerNotified;

    [XafDisplayName("Dealer Notified")]
    [VisibleInListView(false)]
    public virtual bool DealerNotified
    {
        get => _dealerNotified;
        set
        {
            if (_dealerNotified != value)
            {
                RaisePropertyChanging(nameof(DealerNotified));
                _dealerNotified = value;
                RaisePropertyChanged(nameof(DealerNotified));
            }
        }
    }

    #endregion
}
