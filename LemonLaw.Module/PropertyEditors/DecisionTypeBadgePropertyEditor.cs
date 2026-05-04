using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Module.PropertyEditors
{
    /// <summary>
    /// Renders the Decision navigation property as a coloured badge in list views.
    /// The column type is Decision (the entity), so we read DecisionType off it.
    /// </summary>
    [PropertyEditor(typeof(Decision), "DecisionTypeBadgePropertyEditor", false)]
    public class DecisionTypeBadgePropertyEditor : BlazorPropertyEditorBase
    {
        public DecisionTypeBadgePropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            // dataContext is the parent row object (VllApplication).
            // The column's ModelMember.Name is "Decision" — get the Decision nav property,
            // then read DecisionType off it.
            Decision? decision = null;

            if (dataContext != null && Model.ModelMember != null)
            {
                var navProp = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                decision = navProp?.GetValue(dataContext) as Decision;
            }

            // No decision yet — render a neutral dash
            if (decision == null)
            {
                return builder =>
                {
                    builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "style", "color:#d1d5db;font-size:.8125rem;");
                    builder.AddContent(2, "—");
                    builder.CloseElement();
                };
            }

            var (text, bg, fg) = decision.DecisionType switch
            {
                DecisionType.REFUND_ORDERED        => ("Refund Ordered",        "#dcfce7", "#166534"),
                DecisionType.REPLACEMENT_ORDERED   => ("Replacement Ordered",   "#dbeafe", "#1e40af"),
                DecisionType.REIMBURSEMENT_ORDERED => ("Reimbursement Ordered", "#cffafe", "#155e75"),
                DecisionType.CLAIM_DENIED          => ("Claim Denied",          "#fee2e2", "#991b1b"),
                DecisionType.SETTLED_PRIOR         => ("Settled Prior",         "#ede9fe", "#5b21b6"),
                DecisionType.WITHDRAWN             => ("Withdrawn",             "#f1f5f9", "#475569"),
                _                                  => (decision.DecisionType.ToString(), "#f1f5f9", "#475569")
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
