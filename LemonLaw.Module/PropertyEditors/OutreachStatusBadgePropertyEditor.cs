using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(OutreachStatus), "OutreachStatusBadgePropertyEditor", false)]
    public class OutreachStatusBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public OutreachStatusBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = OutreachStatus.PENDING;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is OutreachStatus s)
                    value = s;
            }

            var (text, bg, fg) = value switch
            {
                OutreachStatus.PENDING   => ("Pending",   "#fef3c7", "#92400e"), // amber
                OutreachStatus.SENT      => ("Sent",      "#dbeafe", "#1e40af"), // blue
                OutreachStatus.OPENED    => ("Opened",    "#ede9fe", "#5b21b6"), // purple
                OutreachStatus.RESPONDED => ("Responded", "#dcfce7", "#166534"), // green
                OutreachStatus.OVERDUE   => ("Overdue",   "#fee2e2", "#991b1b"), // red
                OutreachStatus.CLOSED    => ("Closed",    "#f1f5f9", "#475569"), // slate
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
