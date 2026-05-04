using DevExpress.ExpressApp;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
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
                else if (memberType == typeof(ApplicationStatus))
                {
                    column.PropertyEditorType = typeof(ApplicationStatusBadgePropertyEditor);
                }
                else if (memberType == typeof(ApplicationType))
                {
                    column.PropertyEditorType = typeof(ApplicationTypeBadgePropertyEditor);
                }
                else if (memberType == typeof(Decision))
                {
                    // Decision is a navigation property — badge reads DecisionType off the entity
                    column.PropertyEditorType = typeof(DecisionTypeBadgePropertyEditor);
                }
            }
        }
    }
}