using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.DecisionDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    /// <summary>
    /// Custom property editor that renders the full DecisionDetailView Razor component
    /// inside an XAF Detail View. Wired up in Model.xafml by setting
    /// PropertyEditorType="DecisionDetailPropertyEditor" on the DetailPanel item.
    /// </summary>
    [PropertyEditor(typeof(object), "DecisionDetailPropertyEditor", false)]
    public class DecisionDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public DecisionDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new DecisionDetailAdapter(this);
    }

    internal sealed class DecisionDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly DecisionDetailPropertyEditor _editor;

        public DecisionDetailAdapter(DecisionDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var decision = _editor.CurrentObject as Decision;

            builder.OpenComponent<DecisionDetailView>(0);
            builder.AddAttribute(1, nameof(DecisionDetailView.CurrentObject), decision);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;

        public event EventHandler? ValueChanged;

        public object? GetValue() => _editor.PropertyValue;

        public void SetValue(object? value) { /* display-only */ }

        public void Dispose() { }
    }
}
