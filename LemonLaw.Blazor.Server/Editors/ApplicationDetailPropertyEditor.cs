using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.ApplicationDetail;
using Microsoft.AspNetCore.Components;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Blazor.Server.Editors
{
    /// <summary>
    /// Custom property editor that renders the full ApplicationDetailView Razor component
    /// inside an XAF Detail View. Wire it up in Model.DesignedDiffs.xafml by setting
    /// PropertyEditorType="ApplicationDetailPropertyEditor" on the DetailPanel item.
    /// </summary>
    [PropertyEditor(typeof(object), "ApplicationDetailPropertyEditor", false)]
    public class ApplicationDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public ApplicationDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new ApplicationDetailAdapter(this);
    }

    internal sealed class ApplicationDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly ApplicationDetailPropertyEditor _editor;

        public ApplicationDetailAdapter(ApplicationDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var app = _editor.CurrentObject as AppEntity;

            builder.OpenComponent<ApplicationDetailView>(0);
            builder.AddAttribute(1, nameof(ApplicationDetailView.CurrentObject), app);
            builder.CloseComponent();
        };

        // Must return IComponentModel (from DevExpress.ExpressApp.Blazor.Components.Models)
        public IComponentModel? ComponentModel => null;

        public event EventHandler? ValueChanged;

        public object? GetValue() => _editor.PropertyValue;

        public void SetValue(object? value) { /* display-only */ }

        public void Dispose() { }
    }
}
