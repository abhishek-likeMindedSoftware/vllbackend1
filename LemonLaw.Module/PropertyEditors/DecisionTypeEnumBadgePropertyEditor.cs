using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    /// <summary>
    /// Renders the DecisionType enum column as a colour badge in the Decision list view.
    /// (Separate from DecisionTypeBadgePropertyEditor which handles the Decision
    /// navigation property on the Application list view.)
    /// </summary>
    [PropertyEditor(typeof(DecisionType), "DecisionTypeEnumBadgePropertyEditor", false)]
    public class DecisionTypeEnumBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public DecisionTypeEnumBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            var value = DecisionType.CLAIM_DENIED;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is DecisionType d)
                    value = d;
            }

            var (text, bg, fg) = value switch
            {
                DecisionType.REFUND_ORDERED        => ("Refund Ordered",        "#dcfce7", "#166534"),
                DecisionType.REPLACEMENT_ORDERED   => ("Replacement Ordered",   "#dbeafe", "#1e40af"),
                DecisionType.REIMBURSEMENT_ORDERED => ("Reimbursement Ordered", "#cffafe", "#155e75"),
                DecisionType.CLAIM_DENIED          => ("Claim Denied",          "#fee2e2", "#991b1b"),
                DecisionType.SETTLED_PRIOR         => ("Settled Prior",         "#ede9fe", "#5b21b6"),
                DecisionType.WITHDRAWN             => ("Withdrawn",             "#f1f5f9", "#475569"),
                _                                  => (value.ToString(),        "#f1f5f9", "#475569")
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
