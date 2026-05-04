using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(ApplicationType), "ApplicationTypeBadgePropertyEditor", false)]
    public class ApplicationTypeBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public ApplicationTypeBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = ApplicationType.NEW_CAR;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is ApplicationType t)
                    value = t;
            }

            var (text, bg, fg) = value switch
            {
                ApplicationType.NEW_CAR  => ("New Car",  "#dbeafe", "#1e40af"), // blue
                ApplicationType.USED_CAR => ("Used Car", "#fef3c7", "#92400e"), // amber
                ApplicationType.LEASED   => ("Leased",   "#ede9fe", "#5b21b6"), // purple
                _                        => (value.ToString(), "#f1f5f9", "#475569")
            };

            return builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "style",
                    $"display:inline-flex;align-items:center;padding:2px 9px;" +
                    $"border-radius:99px;font-size:.6875rem;font-weight:700;" +
                    $"letter-spacing:.04em;text-transform:uppercase;white-space:nowrap;" +
                    $"background:{bg};color:{fg};");
                builder.AddContent(2, text);
                builder.CloseElement();
            };
        }

        protected override void OnCurrentObjectChanged() => Refresh();
        protected override void OnValueStored() => Refresh();
    }
}
