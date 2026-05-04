using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.CaseNoteDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "CaseNoteDetailPropertyEditor", false)]
    public class CaseNoteDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public CaseNoteDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new CaseNoteDetailAdapter(this);
    }

    internal sealed class CaseNoteDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly CaseNoteDetailPropertyEditor _editor;

        public CaseNoteDetailAdapter(CaseNoteDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var note = _editor.CurrentObject as CaseNote;
            builder.OpenComponent<CaseNoteDetailView>(0);
            builder.AddAttribute(1, nameof(CaseNoteDetailView.CurrentObject), note);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
