using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(ApplicationStatus), "ApplicationStatusBadgePropertyEditor", false)]
    public class ApplicationStatusBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public ApplicationStatusBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = ApplicationStatus.SUBMITTED;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is ApplicationStatus s)
                    value = s;
            }

            var (text, bg, fg) = value switch
            {
                ApplicationStatus.SUBMITTED         => ("Submitted",         "#fef3c7", "#92400e"),
                ApplicationStatus.INCOMPLETE        => ("Incomplete",        "#fee2e2", "#991b1b"),
                ApplicationStatus.ACCEPTED          => ("Accepted",          "#dbeafe", "#1e40af"),
                ApplicationStatus.DEALER_RESPONDED  => ("Dealer Responded",  "#ede9fe", "#5b21b6"),
                ApplicationStatus.HEARING_SCHEDULED => ("Hearing Scheduled", "#fce7f3", "#9d174d"),
                ApplicationStatus.HEARING_COMPLETE  => ("Hearing Complete",  "#cffafe", "#155e75"),
                ApplicationStatus.DECISION_ISSUED   => ("Decision Issued",   "#dcfce7", "#166534"),
                ApplicationStatus.WITHDRAWN         => ("Withdrawn",         "#f1f5f9", "#475569"),
                ApplicationStatus.CLOSED            => ("Closed",            "#f1f5f9", "#475569"),
                _                                   => (value.ToString(),    "#f1f5f9", "#475569")
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
