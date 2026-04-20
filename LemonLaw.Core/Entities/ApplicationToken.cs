using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class ApplicationToken
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual TokenType TokenType { get; set; }
    public virtual string TokenHash { get; set; } = string.Empty;
    public virtual DateTime ExpiresAt { get; set; }
    public virtual DateTime? LastUsedAt { get; set; }
    public virtual bool IsRevoked { get; set; } = false;
    public virtual DateTime? RevokedAt { get; set; }
    public virtual string? RevokedByStaffId { get; set; }
}
