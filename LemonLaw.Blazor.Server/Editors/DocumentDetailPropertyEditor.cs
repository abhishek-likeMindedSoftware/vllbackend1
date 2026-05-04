using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.DocumentDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "DocumentDetailPropertyEditor", false)]
    public class DocumentDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public DocumentDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new DocumentDetailAdapter(this);
    }

    internal sealed class DocumentDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly DocumentDetailPropertyEditor _editor;

        public DocumentDetailAdapter(DocumentDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var document = _editor.CurrentObject as ApplicationDocument;
            builder.OpenComponent<DocumentDetailView>(0);
            builder.AddAttribute(1, nameof(DocumentDetailView.CurrentObject), document);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
