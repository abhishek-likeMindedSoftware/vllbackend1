using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using LemonLaw.Core.Attributes;

namespace LemonLaw.Module.Controllers;

/// <summary>
/// Hides the default XAF audit fields (CreatedDate, CreatedBy, ModifiedDate,
/// ModifiedBy, IsDeleted) on any DetailView whose entity is tagged with
/// [HideXafAuditFields]. Use this on entities that render their own
/// AuditSection Blazor component inside a custom detail panel.
/// </summary>
public class HideAuditFieldsController : ViewController<DetailView>
{
    private static readonly HashSet<string> AuditFieldNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "CreatedDate", "CreatedBy", "ModifiedDate", "ModifiedBy", "IsDeleted"
    };

    protected override void OnActivated()
    {
        base.OnActivated();

        var entityType = View?.ObjectTypeInfo?.Type;
        if (entityType == null) return;

        // Only act on entities tagged with [HideXafAuditFields]
        var hasAttribute = entityType
            .GetCustomAttributes(typeof(HideXafAuditFieldsAttribute), inherit: true)
            .Any();

        if (!hasAttribute) return;

        // Remove audit field editors from the view's item collection
        var toRemove = View.GetItems<PropertyEditor>()
            .Where(pe => AuditFieldNames.Contains(pe.PropertyName))
            .ToList();

        foreach (var pe in toRemove)
            View.RemoveItem(pe.Id);
    }
}
