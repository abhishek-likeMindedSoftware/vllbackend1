using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Defect : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string DefectDescription { get; set; } = string.Empty;
    public virtual DefectCategory DefectCategory { get; set; }
    public virtual DateOnly FirstOccurrenceDate { get; set; }
    public virtual bool IsOngoing { get; set; }
    public virtual int SortOrder { get; set; }
}
