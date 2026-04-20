using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.ComponentModel;

namespace LemonLaw.Core.Entities;

[DefaultProperty(nameof(DefectDescription))]
[XafDisplayName("Defect")]
public class Defect : AuditDetails
{
    [Browsable(false)]
    [VisibleInListView(false)]
    public virtual Guid Id { get; set; }

    [Browsable(false)]
    public virtual Guid ApplicationId { get; set; }

    [Browsable(false)]
    public virtual Application? Application { get; set; }

    private string _defectDescription = string.Empty;
    [XafDisplayName("Description")]
    public virtual string DefectDescription
    {
        get => _defectDescription;
        set { if (_defectDescription != value) { RaisePropertyChanging(nameof(DefectDescription)); _defectDescription = value; RaisePropertyChanged(nameof(DefectDescription)); } }
    }

    [XafDisplayName("Category")]
    public virtual DefectCategory DefectCategory { get; set; }

    [XafDisplayName("First Occurrence")]
    public virtual DateOnly FirstOccurrenceDate { get; set; }

    [XafDisplayName("Ongoing")]
    public virtual bool IsOngoing { get; set; }

    [Browsable(false)]
    public virtual int SortOrder { get; set; }
}
