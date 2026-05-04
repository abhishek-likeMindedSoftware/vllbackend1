using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using LemonLaw.Core.Entities;
using DecisionEntity = LemonLaw.Core.Entities.Decision;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Controls toolbar actions on views that use a custom Blazor detail panel.
    /// Covers Application and Decision (and any future entity added to <see cref="CustomDetailViewTypes"/>).
    ///
    /// List view  → hides New (records are created via portal / case workflow only)
    /// Detail view → hides SaveAndNew and SaveAndClose
    ///               (saves happen via the custom Edit popups inside the Blazor component)
    /// </summary>
    public class ApplicationViewActionsController : ViewController
    {
        /// <summary>
        /// Entity types that use a custom Blazor detail panel.
        /// Add new types here to automatically get the same toolbar behaviour.
        /// </summary>
        private static readonly HashSet<Type> CustomDetailViewTypes = new()
        {
            typeof(VllApplication),
            typeof(DecisionEntity),
            typeof(Defect),
            typeof(RepairAttempt),
            typeof(Expense),
            typeof(Hearing),
            typeof(ApplicationDocument),
            typeof(Correspondence),
            typeof(DealerOutreach),
            typeof(CaseNote),
        };

        protected override void OnActivated()
        {
            base.OnActivated();

            var entityType = View?.ObjectTypeInfo?.Type;
            if (entityType == null || !CustomDetailViewTypes.Contains(entityType))
                return;

            var modificationsController = Frame.GetController<ModificationsController>();
            var newObjectController    = Frame.GetController<NewObjectViewController>();

            if (View is ListView)
            {
                // Hide New button — records are never created directly from the list
                if (newObjectController != null)
                    newObjectController.NewObjectAction.Active["PortalOnly"] = false;
            }
            else if (View is DetailView)
            {
                // Hide SaveAndNew, SaveAndClose, Save — saves go through the Blazor edit popups
                if (modificationsController != null)
                {
                    modificationsController.SaveAndNewAction.Active["PortalOnly"]   = false;
                    modificationsController.SaveAndCloseAction.Active["PortalOnly"] = false;
                    modificationsController.SaveAction.Active["PortalOnly"]         = false;
                }

                // Hide Refresh
                var refreshController = Frame.GetController<DevExpress.ExpressApp.SystemModule.RefreshController>();
                if (refreshController != null)
                    refreshController.RefreshAction.Active["PortalOnly"] = false;

                // Hide Previous/Next navigation arrows — iterate all controllers and deactivate
                // PreviousObject and NextObject actions by their well-known IDs
                foreach (var controller in Frame.Controllers)
                {
                    foreach (var action in controller.Actions)
                    {
                        if (action.Id == "PreviousObject" || action.Id == "NextObject")
                            action.Active["PortalOnly"] = false;
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
