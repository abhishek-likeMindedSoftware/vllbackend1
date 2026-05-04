namespace LemonLaw.Core.Attributes;

/// <summary>
/// Apply to any entity whose XAF detail view uses a custom Blazor component.
/// The HideAuditFieldsController will suppress the default XAF audit fields
/// (CreatedDate, CreatedBy, ModifiedDate, ModifiedBy, IsDeleted) from the
/// detail view layout, since the custom component renders its own AuditSection.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class HideXafAuditFieldsAttribute : Attribute { }
