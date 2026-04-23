using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;


namespace LemonLaw.Module.Controllers
{
    public class LockFieldController : ViewController<DetailView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            LockApplicationIfExists(View.CurrentObject);
            LockFaqIfExists(View.CurrentObject);
        }

        // ---------------- APPLICATION ----------------

        private void LockApplicationIfExists(object currentObject)
        {
            if (currentObject == null)
                return;

            var applicationProperty = currentObject.GetType().GetProperty("Application");
            if (applicationProperty == null)
                return;

            var applicationValue = applicationProperty.GetValue(currentObject);
            if (applicationValue != null)
            {
                LockField("Application");
            }
        }

        // ---------------- FAQ ----------------

        private void LockFaqIfExists(object currentObject)
        {
            if (currentObject == null)
                return;

            var faqProperty = currentObject.GetType().GetProperty("FaqQuestion");
            if (faqProperty == null)
                return;

            var faqValue = faqProperty.GetValue(currentObject);
            if (faqValue != null)
            {
                LockField("FaqQuestion");
            }
        }

        // ---------------- COMMON ----------------

        private void LockField(string propertyName)
        {
            if (View.FindItem(propertyName) is PropertyEditor editor)
            {
                editor.AllowEdit["LockIfSet"] = false;
            }
        }

        protected override void OnDeactivated()
        {
            Unlock("Application");
            Unlock("FaqQuestion");

            base.OnDeactivated();
        }

        private void Unlock(string propertyName)
        {
            if (View.FindItem(propertyName) is PropertyEditor editor)
            {
                editor.AllowEdit.RemoveItem("LockIfSet");
            }
        }
    }
}
