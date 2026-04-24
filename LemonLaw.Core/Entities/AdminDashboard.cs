using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonLaw.Core.Entities;

/// <summary>
/// Dummy non-persistent entity used solely to host the custom dashboard
/// Razor component inside XAF's detail view framework.
/// The Title property is rendered by DashboardPropertyEditor which
/// substitutes the entire view with the Dashboard.razor component.
/// </summary>
[DomainComponent]
[XafDisplayName("Dashboard")]
public class AdminDashboard
{
    [Key]
    [Browsable(false)]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Placeholder property — the DashboardPropertyEditor renders
    /// the full Dashboard Blazor component in place of this field.
    /// </summary>
    [XafDisplayName("")]
    public object? Title { get; set; }
}
