
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using System;

namespace LemonLaw.Module.PropertyEditors
{
    [PropertyEditor(typeof(bool), "BadgeBooleanPropertyEditor", false)]
    public class BadgeBooleanPropertyEditor : BlazorPropertyEditorBase
    {
        public BadgeBooleanPropertyEditor(Type objectType, IModelMemberViewItem model)
        : base(objectType, model)
        {
        }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            bool value = false;

            if (dataContext != null && Model.ModelMember != null)
            {
                var propInfo = dataContext.GetType().GetProperty(Model.ModelMember.Name);
                if (propInfo != null && propInfo.GetValue(dataContext) is bool b)
                    value = b;
            }

            string badgeClass = value
                ? "badge bg-success text-white px-2 py-1 rounded-pill"
                : "badge bg-danger text-white px-2 py-1 rounded-pill";

            string badgeText = value ? "YES" : "NO";

            return builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", badgeClass);
                builder.AddContent(2, badgeText);
                builder.CloseElement();
            };
        }

        protected override void OnCurrentObjectChanged() => Refresh();
        protected override void OnValueStored() => Refresh();
    }
}