using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(ExpenseType))]
[XafDisplayName("Expense")]
public class Expense : AuditDetails,
        INotifyPropertyChanging, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF & INotify
    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanging(string propertyName) =>
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

    protected void RaisePropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                RaisePropertyChanging(nameof(ObjectSpace));
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

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

    private Guid? _applicationId;

    [Browsable(false)]
    public virtual Guid? ApplicationId
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

    [ForeignKey("ApplicationId")]
    [DevExpress.Xpo.Association("Application-Expenses")]
    public virtual Application? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }

    #endregion

    // Caption is referenced by ObjectCaptionFormat="{0:Caption}" in Model.DesignedDiffs.xafml.
    // XAF uses it to set the tab/window title when this record is opened in a detail view,
    // showing "Expense - <CaseNumber>" instead of the DefaultProperty (ExpenseType).
    [NotMapped, Browsable(false)]
    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public string Caption => $"Expense - {Application?.CaseNumber ?? Id.ToString()[..8]}";

    #region Expense Details

    private ExpenseType _expenseType;

    [XafDisplayName("Expense Type")]
    public virtual ExpenseType ExpenseType
    {
        get => _expenseType;
        set
        {
            if (_expenseType != value)
            {
                RaisePropertyChanging(nameof(ExpenseType));
                _expenseType = value;
                RaisePropertyChanged(nameof(ExpenseType));
            }
        }
    }

    private DateOnly? _expenseDate;

    [XafDisplayName("Date")]
    [VisibleInListView(false)]
    public virtual DateOnly? ExpenseDate
    {
        get => _expenseDate;
        set
        {
            if (_expenseDate != value)
            {
                RaisePropertyChanging(nameof(ExpenseDate));
                _expenseDate = value;
                RaisePropertyChanged(nameof(ExpenseDate));
            }
        }
    }

    private decimal _amount;

    [XafDisplayName("Amount ($)")]
    public virtual decimal Amount
    {
        get => _amount;
        set
        {
            if (_amount != value)
            {
                RaisePropertyChanging(nameof(Amount));
                _amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }
    }

    private string? _description;

    [XafDisplayName("Description")]
    [VisibleInListView(false)]
    public virtual string? Description
    {
        get => _description;
        set
        {
            if (_description != value)
            {
                RaisePropertyChanging(nameof(Description));
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
    }

    private bool _receiptUploaded;

    [XafDisplayName("Receipt Uploaded")]
    [VisibleInListView(false)]
    public virtual bool ReceiptUploaded
    {
        get => _receiptUploaded;
        set
        {
            if (_receiptUploaded != value)
            {
                RaisePropertyChanging(nameof(ReceiptUploaded));
                _receiptUploaded = value;
                RaisePropertyChanged(nameof(ReceiptUploaded));
            }
        }
    }

    #endregion
}
