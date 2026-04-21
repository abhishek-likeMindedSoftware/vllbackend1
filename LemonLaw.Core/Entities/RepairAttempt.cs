using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(RepairFacilityName))]
[XafDisplayName("Repair Attempt")]
public class RepairAttempt : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
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

    public RepairAttempt() { }

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

    #region Repair Details

    private DateOnly _repairDate;

    [XafDisplayName("Repair Date")]
    public virtual DateOnly RepairDate
    {
        get => _repairDate;
        set
        {
            if (_repairDate != value)
            {
                RaisePropertyChanging(nameof(RepairDate));
                _repairDate = value;
                RaisePropertyChanged(nameof(RepairDate));
            }
        }
    }

    private string _repairFacilityName = string.Empty;

    [XafDisplayName("Facility Name")]
    public virtual string RepairFacilityName
    {
        get => _repairFacilityName;
        set
        {
            if (_repairFacilityName != value)
            {
                RaisePropertyChanging(nameof(RepairFacilityName));
                _repairFacilityName = value;
                RaisePropertyChanged(nameof(RepairFacilityName));
            }
        }
    }

    private string? _repairFacilityAddr;

    [XafDisplayName("Facility Address")]
    [VisibleInListView(false)]
    public virtual string? RepairFacilityAddr
    {
        get => _repairFacilityAddr;
        set
        {
            if (_repairFacilityAddr != value)
            {
                RaisePropertyChanging(nameof(RepairFacilityAddr));
                _repairFacilityAddr = value;
                RaisePropertyChanged(nameof(RepairFacilityAddr));
            }
        }
    }

    private string? _repairFacilityPlaceId;

    [Browsable(false)]
    public virtual string? RepairFacilityPlaceId
    {
        get => _repairFacilityPlaceId;
        set
        {
            if (_repairFacilityPlaceId != value)
            {
                RaisePropertyChanging(nameof(RepairFacilityPlaceId));
                _repairFacilityPlaceId = value;
                RaisePropertyChanged(nameof(RepairFacilityPlaceId));
            }
        }
    }

    private string? _roNumber;

    [XafDisplayName("RO Number")]
    [VisibleInListView(false)]
    public virtual string? RoNumber
    {
        get => _roNumber;
        set
        {
            if (_roNumber != value)
            {
                RaisePropertyChanging(nameof(RoNumber));
                _roNumber = value;
                RaisePropertyChanged(nameof(RoNumber));
            }
        }
    }

    private int? _mileageAtRepair;

    [XafDisplayName("Mileage at Repair")]
    [VisibleInListView(false)]
    public virtual int? MileageAtRepair
    {
        get => _mileageAtRepair;
        set
        {
            if (_mileageAtRepair != value)
            {
                RaisePropertyChanging(nameof(MileageAtRepair));
                _mileageAtRepair = value;
                RaisePropertyChanged(nameof(MileageAtRepair));
            }
        }
    }

    private string _defectsAddressed = string.Empty;

    [XafDisplayName("Defects Addressed")]
    public virtual string DefectsAddressed
    {
        get => _defectsAddressed;
        set
        {
            if (_defectsAddressed != value)
            {
                RaisePropertyChanging(nameof(DefectsAddressed));
                _defectsAddressed = value;
                RaisePropertyChanged(nameof(DefectsAddressed));
            }
        }
    }

    private bool _repairSuccessful;

    [XafDisplayName("Repair Successful")]
    public virtual bool RepairSuccessful
    {
        get => _repairSuccessful;
        set
        {
            if (_repairSuccessful != value)
            {
                RaisePropertyChanging(nameof(RepairSuccessful));
                _repairSuccessful = value;
                RaisePropertyChanged(nameof(RepairSuccessful));
            }
        }
    }

    private int? _daysOutOfService;

    [XafDisplayName("Days Out of Service")]
    [VisibleInListView(false)]
    public virtual int? DaysOutOfService
    {
        get => _daysOutOfService;
        set
        {
            if (_daysOutOfService != value)
            {
                RaisePropertyChanging(nameof(DaysOutOfService));
                _daysOutOfService = value;
                RaisePropertyChanged(nameof(DaysOutOfService));
            }
        }
    }

    private int _sortOrder;

    [Browsable(false)]
    public virtual int SortOrder
    {
        get => _sortOrder;
        set
        {
            if (_sortOrder != value)
            {
                RaisePropertyChanging(nameof(SortOrder));
                _sortOrder = value;
                RaisePropertyChanged(nameof(SortOrder));
            }
        }
    }

    #endregion
}
