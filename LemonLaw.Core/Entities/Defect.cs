using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DefectDescription))]
[XafDisplayName("Defect")]
public class Defect : AuditDetails, INotifyPropertyChanged, IObjectSpaceLink
{
    #region XAF
    public new event PropertyChangedEventHandler PropertyChanged;
    protected new void RaisePropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private IObjectSpace _objectSpace;

    [NotMapped, Browsable(false)]
    public virtual IObjectSpace ObjectSpace
    {
        get => _objectSpace;
        set
        {
            if (_objectSpace != value)
            {
                _objectSpace = value;
                RaisePropertyChanged(nameof(ObjectSpace));
            }
        }
    }
    #endregion

    public Defect() { }

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
    private Guid _applicationId;

    [Browsable(false)]
    public virtual Guid ApplicationId
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

    [ForeignKey(nameof(ApplicationId))]
    [Browsable(false)]
    public virtual Application? Application
    {
        get => _application;
        set
        {
            if (_application != value)
            {
                RaisePropertyChanging(nameof(Application));
                _application = value;
                ApplicationId = value?.Id ?? Guid.Empty;
                RaisePropertyChanged(nameof(Application));
            }
        }
    }
    #endregion

    #region Defect Details

    private string _defectDescription = string.Empty;

    [XafDisplayName("Description")]
    public virtual string DefectDescription
    {
        get => _defectDescription;
        set
        {
            if (_defectDescription != value)
            {
                RaisePropertyChanging(nameof(DefectDescription));
                _defectDescription = value;
                RaisePropertyChanged(nameof(DefectDescription));
            }
        }
    }

    private DefectCategory _defectCategory;

    [XafDisplayName("Category")]
    public virtual DefectCategory DefectCategory
    {
        get => _defectCategory;
        set
        {
            if (_defectCategory != value)
            {
                RaisePropertyChanging(nameof(DefectCategory));
                _defectCategory = value;
                RaisePropertyChanged(nameof(DefectCategory));
            }
        }
    }

    private DateOnly _firstOccurrenceDate;

    [XafDisplayName("First Occurrence")]
    public virtual DateOnly FirstOccurrenceDate
    {
        get => _firstOccurrenceDate;
        set
        {
            if (_firstOccurrenceDate != value)
            {
                RaisePropertyChanging(nameof(FirstOccurrenceDate));
                _firstOccurrenceDate = value;
                RaisePropertyChanged(nameof(FirstOccurrenceDate));
            }
        }
    }

    private bool _isOngoing;

    [XafDisplayName("Ongoing")]
    public virtual bool IsOngoing
    {
        get => _isOngoing;
        set
        {
            if (_isOngoing != value)
            {
                RaisePropertyChanging(nameof(IsOngoing));
                _isOngoing = value;
                RaisePropertyChanged(nameof(IsOngoing));
            }
        }
    }

    private int _sortOrder;

    [Browsable(false)]
    public virtual int SortOrder
    {
        get => _sortOrder;
        set
        {
            if (_sortOrder != value)
            {
                RaisePropertyChanging(nameof(SortOrder));
                _sortOrder = value;
                RaisePropertyChanged(nameof(SortOrder));
            }
        }
    }

    #endregion
}
