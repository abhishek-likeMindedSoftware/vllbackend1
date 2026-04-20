namespace LemonLaw.Core.Entities;

public class CaseNote : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual string NoteText { get; set; } = string.Empty;
    public virtual string CreatedByStaffId { get; set; } = string.Empty;
    public virtual string CreatedByName { get; set; } = string.Empty;
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual bool IsPinned { get; set; } = false;
}
