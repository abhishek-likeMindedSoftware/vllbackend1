using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(HearingOutcome), "HearingOutcomeBadgePropertyEditor", false)]
    public class HearingOutcomeBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public HearingOutcomeBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = HearingOutcome.PENDING;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is HearingOutcome o)
                    value = o;
            }

            var (text, bg, fg) = value switch
            {
                HearingOutcome.PENDING                => ("Pending",                "#fef3c7", "#92400e"), // amber
                HearingOutcome.SETTLED                => ("Settled",                "#dcfce7", "#166534"), // green
                HearingOutcome.DECISION_FOR_CONSUMER  => ("Decision for Consumer",  "#dbeafe", "#1e40af"), // blue
                HearingOutcome.DECISION_FOR_DEALER    => ("Decision for Dealer",    "#ede9fe", "#5b21b6"), // purple
                HearingOutcome.NO_JURISDICTION        => ("No Jurisdiction",        "#fce7f3", "#9d174d"), // pink
                HearingOutcome.WITHDRAWN              => ("Withdrawn",              "#f1f5f9", "#475569"), // slate
                _                                     => (value.ToString(),         "#f1f5f9", "#475569")
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
