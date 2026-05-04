using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.DefectDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "DefectDetailPropertyEditor", false)]
    public class DefectDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public DefectDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new DefectDetailAdapter(this);
    }

    internal sealed class DefectDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly DefectDetailPropertyEditor _editor;

        public DefectDetailAdapter(DefectDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var defect = _editor.CurrentObject as Defect;
            builder.OpenComponent<DefectDetailView>(0);
            builder.AddAttribute(1, nameof(DefectDetailView.CurrentObject), defect);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
