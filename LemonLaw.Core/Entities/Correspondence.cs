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

[DefaultProperty(nameof(Subject))]
[XafDisplayName("Correspondence")]
public class Correspondence : AuditDetails,
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
    [DevExpress.Xpo.Association("Application-Correspondences")]
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

    #region Correspondence Details

    private CorrespondenceDirection _direction;

    [XafDisplayName("Direction")]
    public virtual CorrespondenceDirection Direction
    {
        get => _direction;
        set
        {
            if (_direction != value)
            {
                RaisePropertyChanging(nameof(Direction));
                _direction = value;
                RaisePropertyChanged(nameof(Direction));
            }
        }
    }

    private CorrespondenceRecipientType _recipientType;

    [XafDisplayName("Recipient Type")]
    public virtual CorrespondenceRecipientType RecipientType
    {
        get => _recipientType;
        set
        {
            if (_recipientType != value)
            {
                RaisePropertyChanging(nameof(RecipientType));
                _recipientType = value;
                RaisePropertyChanged(nameof(RecipientType));
            }
        }
    }

    private string _recipientEmail = string.Empty;

    [XafDisplayName("Recipient Email")]
    public virtual string RecipientEmail
    {
        get => _recipientEmail;
        set
        {
            if (_recipientEmail != value)
            {
                RaisePropertyChanging(nameof(RecipientEmail));
                _recipientEmail = value;
                RaisePropertyChanged(nameof(RecipientEmail));
            }
        }
    }

    private string _subject = string.Empty;

    [XafDisplayName("Subject")]
    public virtual string Subject
    {
        get => _subject;
        set
        {
            if (_subject != value)
            {
                RaisePropertyChanging(nameof(Subject));
                _subject = value;
                RaisePropertyChanged(nameof(Subject));
            }
        }
    }

    private string? _bodyPreview;

    [XafDisplayName("Preview")]
    [VisibleInListView(false)]
    public virtual string? BodyPreview
    {
        get => _bodyPreview;
        set
        {
            if (_bodyPreview != value)
            {
                RaisePropertyChanging(nameof(BodyPreview));
                _bodyPreview = value;
                RaisePropertyChanged(nameof(BodyPreview));
            }
        }
    }

    private string? _templateUsed;

    [XafDisplayName("Template Used")]
    [VisibleInListView(false)]
    public virtual string? TemplateUsed
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

    private string? _sentByStaffId;

    [Browsable(false)]
    public virtual string? SentByStaffId
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

    private DateTime? _deliveryUpdatedAt;

    [XafDisplayName("Delivery Updated")]
    [VisibleInListView(false)]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    public virtual DateTime? DeliveryUpdatedAt
    {
        get => _deliveryUpdatedAt;
        set
        {
            if (_deliveryUpdatedAt != value)
            {
                RaisePropertyChanging(nameof(DeliveryUpdatedAt));
                _deliveryUpdatedAt = value;
                RaisePropertyChanged(nameof(DeliveryUpdatedAt));
            }
        }
    }

    #endregion
}
