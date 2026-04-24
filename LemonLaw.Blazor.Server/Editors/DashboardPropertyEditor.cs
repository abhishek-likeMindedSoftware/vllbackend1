using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Pages;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "DashboardPropertyEditor", false)]
    public class DashboardPropertyEditor : BlazorPropertyEditorBase
    {
        public DashboardPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new DashboardPropertyEditorAdapter(this);
    }

    internal sealed class DashboardPropertyEditorAdapter : IComponentAdapter, IDisposable
    {
        private readonly DashboardPropertyEditor _editor;

        public DashboardPropertyEditorAdapter(DashboardPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            builder.OpenComponent(0, typeof(Dashboard));
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;

        public event EventHandler? ValueChanged;

        public object? GetValue() => _editor.PropertyValue;

        public void SetValue(object? value) { /* display-only */ }

        public void Dispose() { }
    }
}
