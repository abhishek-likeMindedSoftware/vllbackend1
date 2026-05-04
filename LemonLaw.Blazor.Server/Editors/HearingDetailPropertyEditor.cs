using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.HearingDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "HearingDetailPropertyEditor", false)]
    public class HearingDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public HearingDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new HearingDetailAdapter(this);
    }

    internal sealed class HearingDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly HearingDetailPropertyEditor _editor;

        public HearingDetailAdapter(HearingDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var hearing = _editor.CurrentObject as Hearing;
            builder.OpenComponent<HearingDetailView>(0);
            builder.AddAttribute(1, nameof(HearingDetailView.CurrentObject), hearing);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
