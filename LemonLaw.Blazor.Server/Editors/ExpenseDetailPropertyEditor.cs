using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using LemonLaw.Blazor.Server.Components.ExpenseDetail;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;

namespace LemonLaw.Blazor.Server.Editors
{
    [PropertyEditor(typeof(object), "ExpenseDetailPropertyEditor", false)]
    public class ExpenseDetailPropertyEditor : BlazorPropertyEditorBase
    {
        public ExpenseDetailPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) { }

        protected override IComponentAdapter CreateComponentAdapter()
            => new ExpenseDetailAdapter(this);
    }

    internal sealed class ExpenseDetailAdapter : IComponentAdapter, IDisposable
    {
        private readonly ExpenseDetailPropertyEditor _editor;

        public ExpenseDetailAdapter(ExpenseDetailPropertyEditor editor)
        {
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public RenderFragment ComponentContent => builder =>
        {
            var expense = _editor.CurrentObject as Expense;
            builder.OpenComponent<ExpenseDetailView>(0);
            builder.AddAttribute(1, nameof(ExpenseDetailView.CurrentObject), expense);
            builder.CloseComponent();
        };

        public IComponentModel? ComponentModel => null;
        public event EventHandler? ValueChanged;
        public object? GetValue() => _editor.PropertyValue;
        public void SetValue(object? value) { }
        public void Dispose() { }
    }
}
