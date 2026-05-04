using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using LemonLaw.Core.Attributes;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(HearingDate))]
[NavigationItem("Hearings")]
[XafDisplayName("Hearing")]
[HideXafAuditFields]
public class Hearing : AuditDetails,
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

    private VllApplication? _application;

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-Hearings")]
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

    // Caption is referenced by ObjectCaptionFormat="{0:Caption}" in Model.DesignedDiffs.xafml.
    // XAF uses it to set the tab/window title when this record is opened in a detail view,
    // showing "Hearing - <CaseNumber>" instead of the DefaultProperty (HearingDate).
    [NotMapped, Browsable(false)]
    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public string Caption => $"Hearing - {Application?.CaseNumber ?? Id.ToString()[..8]}";

    #region Hearing Details

    private DateTime _hearingDate;

    [XafDisplayName("Hearing Date")]
    [VisibleInDetailView(false)]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
    public virtual DateTime HearingDate
    {
        get => _hearingDate;
        set
        {
            if (_hearingDate != value)
            {
                RaisePropertyChanging(nameof(HearingDate));
                _hearingDate = value;
                RaisePropertyChanged(nameof(HearingDate));
            }
        }
    }

    private HearingFormat _hearingFormat;

    [XafDisplayName("Format")]
    [VisibleInDetailView(false)]
    public virtual HearingFormat HearingFormat
    {
        get => _hearingFormat;
        set
        {
            if (_hearingFormat != value)
            {
                RaisePropertyChanging(nameof(HearingFormat));
                _hearingFormat = value;
                RaisePropertyChanged(nameof(HearingFormat));
            }
        }
    }

    private string? _hearingLocation;

    [XafDisplayName("Location / URL")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual string? HearingLocation
    {
        get => _hearingLocation;
        set
        {
            if (_hearingLocation != value)
            {
                RaisePropertyChanging(nameof(HearingLocation));
                _hearingLocation = value;
                RaisePropertyChanged(nameof(HearingLocation));
            }
        }
    }

    private string? _arbitratorName;

    [XafDisplayName("Arbitrator")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual string? ArbitratorName
    {
        get => _arbitratorName;
        set
        {
            if (_arbitratorName != value)
            {
                RaisePropertyChanging(nameof(ArbitratorName));
                _arbitratorName = value;
                RaisePropertyChanged(nameof(ArbitratorName));
            }
        }
    }

    private bool _consumerNoticesSent;

    [XafDisplayName("Consumer Notified")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual bool ConsumerNoticesSent
    {
        get => _consumerNoticesSent;
        set
        {
            if (_consumerNoticesSent != value)
            {
                RaisePropertyChanging(nameof(ConsumerNoticesSent));
                _consumerNoticesSent = value;
                RaisePropertyChanged(nameof(ConsumerNoticesSent));
            }
        }
    }

    private bool _dealerNoticesSent;

    [XafDisplayName("Dealer Notified")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual bool DealerNoticesSent
    {
        get => _dealerNoticesSent;
        set
        {
            if (_dealerNoticesSent != value)
            {
                RaisePropertyChanging(nameof(DealerNoticesSent));
                _dealerNoticesSent = value;
                RaisePropertyChanged(nameof(DealerNoticesSent));
            }
        }
    }

    private HearingOutcome _outcome = HearingOutcome.PENDING;

    [XafDisplayName("Outcome")]
    [VisibleInDetailView(false)]
    public virtual HearingOutcome Outcome
    {
        get => _outcome;
        set
        {
            if (_outcome != value)
            {
                RaisePropertyChanging(nameof(Outcome));
                _outcome = value;
                RaisePropertyChanged(nameof(Outcome));
            }
        }
    }

    private string? _outcomeNotes;

    [XafDisplayName("Outcome Notes")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    public virtual string? OutcomeNotes
    {
        get => _outcomeNotes;
        set
        {
            if (_outcomeNotes != value)
            {
                RaisePropertyChanging(nameof(OutcomeNotes));
                _outcomeNotes = value;
                RaisePropertyChanged(nameof(OutcomeNotes));
            }
        }
    }

    private DateTime? _continuedTo;

    [XafDisplayName("Continued To")]
    [VisibleInDetailView(false), VisibleInListView(false)]
    [ModelDefault("DisplayFormat", "{0:MM/dd/yyyy hh:mm tt}")]
    [ModelDefault("EditMask", "MM/dd/yyyy hh:mm tt")]
    public virtual DateTime? ContinuedTo
    {
        get => _continuedTo;
        set
        {
            if (_continuedTo != value)
            {
                RaisePropertyChanging(nameof(ContinuedTo));
                _continuedTo = value;
                RaisePropertyChanged(nameof(ContinuedTo));
            }
        }
    }

    #endregion

    #region Custom Detail View Hook

    [NotMapped]
    [XafDisplayName("Hearing Detail")]
    [VisibleInDetailView(true), VisibleInListView(false)]
    public object? DetailPanel => this;

    #endregion
}
