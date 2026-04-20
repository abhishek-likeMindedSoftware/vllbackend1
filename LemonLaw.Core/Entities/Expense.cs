using LemonLaw.Core.Enums;

namespace LemonLaw.Core.Entities;

public class Expense : AuditDetails
{
    public virtual Guid Id { get; set; }
    public virtual Guid ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public virtual ExpenseType ExpenseType { get; set; }
    public virtual DateOnly? ExpenseDate { get; set; }
    public virtual decimal Amount { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool ReceiptUploaded { get; set; } = false;
}
