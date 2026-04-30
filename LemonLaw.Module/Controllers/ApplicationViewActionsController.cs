using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Controls toolbar actions on Application views.
    /// - List view: hides New (applications are created via consumer portal only)
    /// - Detail view: hides SaveAndNew
    /// </summary>
    public class ApplicationViewActionsController : ViewController
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            // Only apply to Application views
            if (View?.ObjectTypeInfo?.Type != typeof(AppEntity))
                return;

            var modificationsController = Frame.GetController<ModificationsController>();
            var newObjectController = Frame.GetController<NewObjectViewController>();

            if (View is ListView)
            {
                // Hide New button on the list view
                if (newObjectController != null)
                    newObjectController.NewObjectAction.Active["PortalOnly"] = false;
            }
            else if (View is DetailView)
            {
                // Hide SaveAndNew and SaveAndClose on the detail view
                // (saves happen via our custom Edit popups, not XAF toolbar)
                if (modificationsController != null)
                {
                    modificationsController.SaveAndNewAction.Active["PortalOnly"] = false;
                    modificationsController.SaveAndCloseAction.Active["PortalOnly"] = false;
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
