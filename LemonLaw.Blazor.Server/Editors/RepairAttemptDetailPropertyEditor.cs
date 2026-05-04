using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.RepairAttemptDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "RepairAttemptDetailPropertyEditor", false)]
    public class RepairAttemptDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public RepairAttemptDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new RepairAttemptDetailAdapter(this);
    }

    internal sealed class RepairAttemptDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly RepairAttemptDetailPropertyEditor _editor;

        public RepairAttemptDetailAdapter(RepairAttemptDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var repair = _editor.CurrentObject as RepairAttempt;
            builder.OpenComponent<RepairAttemptDetailView>(0);
            builder.AddAttribute(1, nameof(RepairAttemptDetailView.CurrentObject), repair);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
