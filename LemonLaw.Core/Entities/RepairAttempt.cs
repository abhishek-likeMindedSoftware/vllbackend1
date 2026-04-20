using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(RepairFacilityName))]
[XafDisplayName("Repair Attempt")]
public class RepairAttempt : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    [XafDisplayName("Repair Date")]
    public virtual DateOnly RepairDate { get; set; }

    private string _repairFacilityName = string.Empty;
    [XafDisplayName("Facility Name")]
    public virtual string RepairFacilityName
    {
        get => _repairFacilityName;
        set { if (_repairFacilityName != value) { RaisePropertyChanging(nameof(RepairFacilityName)); _repairFacilityName = value; RaisePropertyChanged(nameof(RepairFacilityName)); } }
    }

    [XafDisplayName("Facility Address")]
    [VisibleInListView(false)]
    public virtual string? RepairFacilityAddr { get; set; }

    [Browsable(false)]
    public virtual string? RepairFacilityPlaceId { get; set; }

    [XafDisplayName("RO Number")]
    [VisibleInListView(false)]
    public virtual string? RoNumber { get; set; }

    [XafDisplayName("Mileage at Repair")]
    [VisibleInListView(false)]
    public virtual int? MileageAtRepair { get; set; }

    [XafDisplayName("Defects Addressed")]
    public virtual string DefectsAddressed { get; set; } = string.Empty;

    [XafDisplayName("Repair Successful")]
    public virtual bool RepairSuccessful { get; set; }

    [XafDisplayName("Days Out of Service")]
    [VisibleInListView(false)]
    public virtual int? DaysOutOfService { get; set; }

    [Browsable(false)]
    public virtual int SortOrder { get; set; }
}
