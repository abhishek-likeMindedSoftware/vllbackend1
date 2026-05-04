using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.CorrespondenceDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "CorrespondenceDetailPropertyEditor", false)]
    public class CorrespondenceDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public CorrespondenceDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new CorrespondenceDetailAdapter(this);
    }

    internal sealed class CorrespondenceDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly CorrespondenceDetailPropertyEditor _editor;

        public CorrespondenceDetailAdapter(CorrespondenceDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var correspondence = _editor.CurrentObject as Correspondence;
            builder.OpenComponent<CorrespondenceDetailView>(0);
            builder.AddAttribute(1, nameof(CorrespondenceDetailView.CurrentObject), correspondence);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
