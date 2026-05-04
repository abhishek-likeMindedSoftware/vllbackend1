using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(OutreachType), "OutreachTypeBadgePropertyEditor", false)]
    public class OutreachTypeBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public OutreachTypeBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = OutreachType.INITIAL;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is OutreachType t)
                    value = t;
            }

            var (text, bg, fg) = value switch
            {
                OutreachType.INITIAL      => ("Initial",      "#dbeafe", "#1e40af"), // blue
                OutreachType.FOLLOW_UP_1  => ("Follow-Up 1",  "#fef3c7", "#92400e"), // amber
                OutreachType.FOLLOW_UP_2  => ("Follow-Up 2",  "#ffedd5", "#9a3412"), // orange
                OutreachType.FINAL_NOTICE => ("Final Notice",  "#fee2e2", "#991b1b"), // red
                _                         => (value.ToString(), "#f1f5f9", "#475569")
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
