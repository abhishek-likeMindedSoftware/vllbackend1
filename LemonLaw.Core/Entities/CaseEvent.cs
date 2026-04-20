using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class CaseEvent
{
    public virtual Guid CaseEventId { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual CaseEventType EventType { get; set; }
    public virtual DateTime EventTimestamp { get; set; } = DateTime.UtcNow;
    public virtual ActorType ActorType { get; set; }
    public virtual string? ActorId { get; set; }
    public virtual string ActorDisplayName { get; set; } = string.Empty;
    public virtual string Description { get; set; } = string.Empty;
    public virtual string? PreviousValue { get; set; }
    public virtual string? NewValue { get; set; }
}
