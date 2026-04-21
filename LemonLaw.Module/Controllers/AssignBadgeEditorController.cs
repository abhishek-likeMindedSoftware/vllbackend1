using DevExpress.ExpressApp;
using LemonLaw.Module.PropertyEditors;

namespace LemonLaw.Module.Controllers
{
    public class AssignBadgeEditorController : ViewController<ListView>
    {
        public AssignBadgeEditorController()
        {
            TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            foreach (var column in View.Model.Columns)
            {
                var memberType = column.ModelMember?.Type;

                if (memberType == typeof(bool))
                {
                    column.PropertyEditorType = typeof(BadgeBooleanPropertyEditor);
                }
            }
        }
    }
}