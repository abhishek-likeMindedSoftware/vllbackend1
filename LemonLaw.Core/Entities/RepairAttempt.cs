namespace LemonLaw.Core.Entities;

public class RepairAttempt : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual DateOnly RepairDate { get; set; }
    public virtual string RepairFacilityName { get; set; } = string.Empty;
    public virtual string? RepairFacilityAddr { get; set; }
    public virtual string? RepairFacilityPlaceId { get; set; }
    public virtual string? RoNumber { get; set; }
    public virtual int? MileageAtRepair { get; set; }
    public virtual string DefectsAddressed { get; set; } = string.Empty;
    public virtual bool RepairSuccessful { get; set; }
    public virtual int? DaysOutOfService { get; set; }
    public virtual int SortOrder { get; set; }
}
