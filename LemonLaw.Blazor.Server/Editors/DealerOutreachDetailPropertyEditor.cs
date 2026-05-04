using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.DealerOutreachDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "DealerOutreachDetailPropertyEditor", false)]
    public class DealerOutreachDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public DealerOutreachDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new DealerOutreachDetailAdapter(this);
    }

    internal sealed class DealerOutreachDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly DealerOutreachDetailPropertyEditor _editor;

        public DealerOutreachDetailAdapter(DealerOutreachDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var outreach = _editor.CurrentObject as DealerOutreach;
            builder.OpenComponent<DealerOutreachDetailView>(0);
            builder.AddAttribute(1, nameof(DealerOutreachDetailView.CurrentObject), outreach);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
