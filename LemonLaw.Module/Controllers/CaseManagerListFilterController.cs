using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using LemonLaw.Core.Entities;

using XafAppUser   = LemonLaw.Core.Entities.ApplicationUser;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Scopes all list views to the CASE_MANAGER's assigned cases.
    ///
    /// - Application list  → AssignedToStaffId = currentUser.UserName
    /// - Hearing list      → Application.AssignedToStaffId = currentUser.UserName
    /// - DealerOutreach list → Application.AssignedToStaffId = currentUser.UserName
    ///
    /// ADMIN and SUPERVISOR are unaffected — they see everything.
    /// The filter is applied via CollectionSource.Criteria (the same pattern used
    /// in the FILIR project) and cleaned up on deactivation.
    /// </summary>
    public sealed class CaseManagerApplicationFilterController
        : ObjectViewController<ListView, VllApplication>
    {
        private const string FilterKey = "CaseManagerFilter";

        protected override void OnActivated()
        {
            base.OnActivated();
            ApplyFilter("AssignedToStaffId = ?");
        }

        protected override void OnDeactivated()
        {
            View?.CollectionSource?.Criteria.Remove(FilterKey);
            base.OnDeactivated();
        }

        private void ApplyFilter(string criteriaTemplate)
        {
            if (View?.CollectionSource == null) return;
            View.CollectionSource.Criteria.Remove(FilterKey);

            var userName = GetCaseManagerUserName();
            if (userName == null) return;

            View.CollectionSource.Criteria[FilterKey] =
                CriteriaOperator.Parse(criteriaTemplate, userName);
        }

        private static string? GetCaseManagerUserName()
        {
            var user = DevExpress.ExpressApp.SecuritySystem.CurrentUser as XafAppUser;
            if (user == null) return null;

            var isCaseManager = user.Roles.Any(r => r.Name == "CASE_MANAGER") &&
                                !user.Roles.Any(r => r.Name == "OCABR_ADMIN" || r.Name == "SUPERVISOR");

            return isCaseManager ? user.UserName : null;
        }
    }

    public sealed class CaseManagerHearingFilterController
        : ObjectViewController<ListView, Hearing>
    {
        private const string FilterKey = "CaseManagerFilter";

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View?.CollectionSource == null) return;
            View.CollectionSource.Criteria.Remove(FilterKey);

            var userName = GetCaseManagerUserName();
            if (userName == null) return;

            View.CollectionSource.Criteria[FilterKey] =
                CriteriaOperator.Parse("Application.AssignedToStaffId = ?", userName);
        }

        protected override void OnDeactivated()
        {
            View?.CollectionSource?.Criteria.Remove(FilterKey);
            base.OnDeactivated();
        }

        private static string? GetCaseManagerUserName()
        {
            var user = DevExpress.ExpressApp.SecuritySystem.CurrentUser as XafAppUser;
            if (user == null) return null;

            var isCaseManager = user.Roles.Any(r => r.Name == "CASE_MANAGER") &&
                                !user.Roles.Any(r => r.Name == "OCABR_ADMIN" || r.Name == "SUPERVISOR");

            return isCaseManager ? user.UserName : null;
        }
    }

    public sealed class CaseManagerDealerOutreachFilterController
        : ObjectViewController<ListView, DealerOutreach>
    {
        private const string FilterKey = "CaseManagerFilter";

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View?.CollectionSource == null) return;
            View.CollectionSource.Criteria.Remove(FilterKey);

            var userName = GetCaseManagerUserName();
            if (userName == null) return;

            View.CollectionSource.Criteria[FilterKey] =
                CriteriaOperator.Parse("Application.AssignedToStaffId = ?", userName);
        }

        protected override void OnDeactivated()
        {
            View?.CollectionSource?.Criteria.Remove(FilterKey);
            base.OnDeactivated();
        }

        private static string? GetCaseManagerUserName()
        {
            var user = DevExpress.ExpressApp.SecuritySystem.CurrentUser as XafAppUser;
            if (user == null) return null;

            var isCaseManager = user.Roles.Any(r => r.Name == "CASE_MANAGER") &&
                                !user.Roles.Any(r => r.Name == "OCABR_ADMIN" || r.Name == "SUPERVISOR");

            return isCaseManager ? user.UserName : null;
        }
    }
}
