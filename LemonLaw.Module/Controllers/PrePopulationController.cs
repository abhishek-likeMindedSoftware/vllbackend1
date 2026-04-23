using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using LemonLaw.Module.Helpers;

namespace LemonLaw.Module.Controllers
{
    public class PrePopulationController : ViewController<ListView>
    {
        private NewObjectViewController _newObjectController;

        protected override void OnActivated()
        {
            base.OnActivated();

            if (Frame is not NestedFrame)
                return;

            _newObjectController = Frame.GetController<NewObjectViewController>();
            if (_newObjectController != null)
                _newObjectController.ObjectCreated += OnObjectCreated;
        }

        private void OnObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if (Frame is not NestedFrame nestedFrame)
                return;

            var parent = nestedFrame.ViewItem?.View?.CurrentObject;
            var child = e.CreatedObject;

            PrePopulationHelper.Apply(parent, child);
        }

        protected override void OnDeactivated()
        {
            if (_newObjectController != null)
                _newObjectController.ObjectCreated -= OnObjectCreated;

            base.OnDeactivated();
        }
    }
}