using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using LemonLaw.Core.Entities;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Hides toolbar actions (Save, Delete, Refresh, navigation arrows) on the AdminDashboard view.
    /// The dashboard is read-only and needs no CRUD buttons.
    /// </summary>
    public class DashboardActionsController : ViewController<DetailView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View?.CurrentObject is not AdminDashboard)
                return;

            // Hide Save / SaveAndClose / SaveAndNew / Cancel
            var saveController = Frame.GetController<ModificationsController>();
            if (saveController != null)
            {
                saveController.SaveAction.Active["DashboardHide"] = false;
                saveController.SaveAndCloseAction.Active["DashboardHide"] = false;
                saveController.SaveAndNewAction.Active["DashboardHide"] = false;
                saveController.CancelAction.Active["DashboardHide"] = false;
            }

            // Hide Delete
            var deleteController = Frame.GetController<DeleteObjectsViewController>();
            if (deleteController != null)
                deleteController.DeleteAction.Active["DashboardHide"] = false;

            // Hide Refresh
            var refreshController = Frame.GetController<RefreshController>();
            if (refreshController != null)
                refreshController.RefreshAction.Active["DashboardHide"] = false;

            // Hide Previous / Next navigation arrows — find by action ID across all controllers
            foreach (var controller in Frame.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    if (action.Id is "PreviousObject" or "NextObject")
                        action.Active["DashboardHide"] = false;
                }
            }
        }

        protected override void OnDeactivated()
        {
            // Restore all actions when leaving the dashboard view
            var saveController = Frame.GetController<ModificationsController>();
            if (saveController != null)
            {
                saveController.SaveAction.Active.RemoveItem("DashboardHide");
                saveController.SaveAndCloseAction.Active.RemoveItem("DashboardHide");
                saveController.SaveAndNewAction.Active.RemoveItem("DashboardHide");
                saveController.CancelAction.Active.RemoveItem("DashboardHide");
            }

            var deleteController = Frame.GetController<DeleteObjectsViewController>();
            if (deleteController != null)
                deleteController.DeleteAction.Active.RemoveItem("DashboardHide");

            var refreshController = Frame.GetController<RefreshController>();
            if (refreshController != null)
                refreshController.RefreshAction.Active.RemoveItem("DashboardHide");

            // Restore nav arrows
            foreach (var controller in Frame.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    if (action.Id is "PreviousObject" or "NextObject")
                        action.Active.RemoveItem("DashboardHide");
                }
            }

            base.OnDeactivated();
        }
    }
}
