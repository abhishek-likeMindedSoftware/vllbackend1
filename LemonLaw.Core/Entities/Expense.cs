using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(ExpenseType))]
[XafDisplayName("Expense")]
public class Expense : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    [XafDisplayName("Expense Type")]
    public virtual ExpenseType ExpenseType { get; set; }

    [XafDisplayName("Date")]
    [VisibleInListView(false)]
    public virtual DateOnly? ExpenseDate { get; set; }

    private decimal _amount;
    [XafDisplayName("Amount ($)")]
    public virtual decimal Amount
    {
        get => _amount;
        set { if (_amount != value) { RaisePropertyChanging(nameof(Amount)); _amount = value; RaisePropertyChanged(nameof(Amount)); } }
    }

    [XafDisplayName("Description")]
    [VisibleInListView(false)]
    public virtual string? Description { get; set; }

    [XafDisplayName("Receipt Uploaded")]
    [VisibleInListView(false)]
    public virtual bool ReceiptUploaded { get; set; } = false;
}
