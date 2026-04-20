using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Correspondence : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual CorrespondenceDirection Direction { get; set; }
    public virtual CorrespondenceRecipientType RecipientType { get; set; }
    public virtual string RecipientEmail { get; set; } = string.Empty;
    public virtual string Subject { get; set; } = string.Empty;
    public virtual string? BodyPreview { get; set; }
    public virtual string? TemplateUsed { get; set; }
    public virtual DateTime? SentAt { get; set; }
    public virtual string? SentByStaffId { get; set; }
    public virtual string? SendGridMessageId { get; set; }
    public virtual EmailDeliveryStatus DeliveryStatus { get; set; } = EmailDeliveryStatus.PENDING;
    public virtual DateTime? DeliveryUpdatedAt { get; set; }
}
