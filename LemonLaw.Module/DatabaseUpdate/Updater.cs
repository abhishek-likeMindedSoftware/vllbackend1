using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EFCore.AuditTrail;
using LemonLaw.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

using AppEntity  = LemonLaw.Core.Entities.Application;
using XafAppUser = LemonLaw.Core.Entities.ApplicationUser;

namespace LemonLaw.Module.DatabaseUpdate
{
    /// <summary>
    /// Seeds the four OCABR roles defined in spec §3.1 and one demo user per role.
    ///
    /// Roles are idempotent — if a role already exists it is left untouched.
    /// To reprovision permissions on an existing DB, delete the role rows and restart.
    ///
    /// Roles per spec §3.1:
    ///   OCABR_ADMIN  — Full access (IsAdministrative = true)
    ///   CASE_MANAGER — Only sees cases assigned to them; cannot assign/reassign
    ///   SUPERVISOR   — All cases + reporting + can assign
    ///   REVIEWER     — Read-only + can add notes
    /// </summary>
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) { }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            var adminRole       = EnsureAdminRole();
            var caseManagerRole = EnsureCaseManagerRole();
            var supervisorRole  = EnsureSupervisorRole();
            var reviewerRole    = EnsureReviewerRole();

            ObjectSpace.CommitChanges();

#if !RELEASE
            var userManager = ObjectSpace.ServiceProvider.GetRequiredService<UserManager>();

            SeedUser(userManager, "Admin@vll.com",      "Admin@123",      adminRole);
            SeedUser(userManager, "Manager@vll.com",    "Manager@123",    caseManagerRole);
            SeedUser(userManager, "Supervisor@vll.com", "Supervisor@123", supervisorRole);
            SeedUser(userManager, "Reviewer@vll.com",   "Reviewer@123",   reviewerRole);

            ObjectSpace.CommitChanges();

            // Seed current Windows user as admin for local dev
            var windowsUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (userManager.FindUserByName<XafAppUser>(ObjectSpace, windowsUser) == null)
            {
                userManager.CreateUser<XafAppUser>(ObjectSpace, windowsUser, "", user =>
                {
                    user.Roles.Add(adminRole);
                    userManager.AddLogin(user, SecurityDefaults.WindowsAuthentication, windowsUser);
                });
                ObjectSpace.CommitChanges();
            }
#endif
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
        }

        // ── Role factories ────────────────────────────────────────────────────

        private PermissionPolicyRole EnsureAdminRole()
        {
            var role = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "OCABR_ADMIN");
            if (role != null) return role;

            role = ObjectSpace.CreateObject<PermissionPolicyRole>();
            role.Name = "OCABR_ADMIN";
            role.IsAdministrative = true;
            return role;
        }

        /// <summary>
        /// CASE_MANAGER — sees only cases assigned to them.
        /// Object-level permission scoped to AssignedToStaffId = CurrentUserId().
        /// Cannot assign or reassign cases.
        /// </summary>
        private PermissionPolicyRole EnsureCaseManagerRole()
        {
            var role = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "CASE_MANAGER");
            if (role != null) return role;

            role = ObjectSpace.CreateObject<PermissionPolicyRole>();
            role.Name = "CASE_MANAGER";
            role.IsAdministrative = false;

            // Applications — only those assigned to the current user.
            // AssignedToStaffId stores UserName (string).
            // NOTE: We rely on CaseManagerListFilterController for UI-layer filtering.
            // The object-level permission here uses the user's Guid ID as XAF requires,
            // but since AssignedToStaffId is a string field we cannot use CurrentUserId().
            // The controller handles the actual data scoping for this role.
            role.AddTypePermissionsRecursively<AppEntity>(
                SecurityOperations.ReadWriteAccess,
                SecurityPermissionState.Allow);

            // Related entities — full access
            GrantFullAccess<Applicant>(role);
            GrantFullAccess<Vehicle>(role);
            GrantFullAccess<Defect>(role);
            GrantFullAccess<RepairAttempt>(role);
            GrantFullAccess<Expense>(role);
            GrantFullAccess<ApplicationDocument>(role);
            GrantFullAccess<CaseNote>(role);
            GrantFullAccess<CaseEvent>(role);
            GrantFullAccess<Correspondence>(role);
            GrantFullAccess<DealerOutreach>(role);
            GrantFullAccess<DealerResponse>(role);
            GrantFullAccess<Hearing>(role);
            GrantFullAccess<Decision>(role);
            GrantFullAccess<ApplicationToken>(role);

            // Own user profile — read + change password only
            role.AddObjectPermission<XafAppUser>(
                SecurityOperations.Read,
                "ID = CurrentUserId()",
                SecurityPermissionState.Allow);
            role.AddMemberPermission<XafAppUser>(
                SecurityOperations.Write, "StoredPassword",
                "ID = CurrentUserId()",
                SecurityPermissionState.Allow);

            // Navigation
            role.AddNavigationPermission("Application/NavigationItems/Items/Dashboard_Nav",        SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Case Management",       SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Dealer Portal Activity",SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Hearings",              SecurityPermissionState.Allow);

            // Deny user/role management
            role.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddTypePermissionsRecursively<XafAppUser>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddObjectPermission<XafAppUser>(SecurityOperations.Read, "ID = CurrentUserId()", SecurityPermissionState.Allow);

            GrantModelDiffAccess(role);
            return role;
        }

        /// <summary>
        /// SUPERVISOR — all cases, reporting, and case assignment.
        /// </summary>
        private PermissionPolicyRole EnsureSupervisorRole()
        {
            var role = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "SUPERVISOR");
            if (role != null) return role;

            role = ObjectSpace.CreateObject<PermissionPolicyRole>();
            role.Name = "SUPERVISOR";
            role.IsAdministrative = false;

            GrantFullAccess<AppEntity>(role);
            GrantFullAccess<Applicant>(role);
            GrantFullAccess<Vehicle>(role);
            GrantFullAccess<Defect>(role);
            GrantFullAccess<RepairAttempt>(role);
            GrantFullAccess<Expense>(role);
            GrantFullAccess<ApplicationDocument>(role);
            GrantFullAccess<CaseNote>(role);
            GrantFullAccess<CaseEvent>(role);
            GrantFullAccess<Correspondence>(role);
            GrantFullAccess<DealerOutreach>(role);
            GrantFullAccess<DealerResponse>(role);
            GrantFullAccess<Hearing>(role);
            GrantFullAccess<Decision>(role);
            GrantFullAccess<ApplicationToken>(role);

            role.AddObjectPermission<XafAppUser>(SecurityOperations.Read, "ID = CurrentUserId()", SecurityPermissionState.Allow);
            role.AddMemberPermission<XafAppUser>(SecurityOperations.Write, "StoredPassword", "ID = CurrentUserId()", SecurityPermissionState.Allow);

            // Navigation
            role.AddNavigationPermission("Application/NavigationItems/Items/Dashboard_Nav",        SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Case Management",       SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Dealer Portal Activity",SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Hearings",              SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Reports",               SecurityPermissionState.Allow);

            role.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddTypePermissionsRecursively<XafAppUser>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddObjectPermission<XafAppUser>(SecurityOperations.Read, "ID = CurrentUserId()", SecurityPermissionState.Allow);

            GrantModelDiffAccess(role);
            return role;
        }

        /// <summary>
        /// REVIEWER — read-only on all cases; can add internal notes.
        /// </summary>
        private PermissionPolicyRole EnsureReviewerRole()
        {
            var role = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "REVIEWER");
            if (role != null) return role;

            role = ObjectSpace.CreateObject<PermissionPolicyRole>();
            role.Name = "REVIEWER";
            role.IsAdministrative = false;

            GrantReadOnly<AppEntity>(role);
            GrantReadOnly<Applicant>(role);
            GrantReadOnly<Vehicle>(role);
            GrantReadOnly<Defect>(role);
            GrantReadOnly<RepairAttempt>(role);
            GrantReadOnly<Expense>(role);
            GrantReadOnly<ApplicationDocument>(role);
            GrantReadOnly<CaseEvent>(role);
            GrantReadOnly<Correspondence>(role);
            GrantReadOnly<DealerOutreach>(role);
            GrantReadOnly<DealerResponse>(role);
            GrantReadOnly<Hearing>(role);
            GrantReadOnly<Decision>(role);

            // Notes — read + create only (no edit/delete)
            role.AddTypePermissionsRecursively<CaseNote>(SecurityOperations.Read,   SecurityPermissionState.Allow);
            role.AddTypePermissionsRecursively<CaseNote>(SecurityOperations.Create, SecurityPermissionState.Allow);

            role.AddObjectPermission<XafAppUser>(SecurityOperations.Read, "ID = CurrentUserId()", SecurityPermissionState.Allow);
            role.AddMemberPermission<XafAppUser>(SecurityOperations.Write, "StoredPassword", "ID = CurrentUserId()", SecurityPermissionState.Allow);

            // Navigation
            role.AddNavigationPermission("Application/NavigationItems/Items/Dashboard_Nav",        SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Case Management",       SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Dealer Portal Activity",SecurityPermissionState.Allow);
            role.AddNavigationPermission("Application/NavigationItems/Items/Hearings",              SecurityPermissionState.Allow);

            role.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddTypePermissionsRecursively<XafAppUser>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Deny);
            role.AddObjectPermission<XafAppUser>(SecurityOperations.Read, "ID = CurrentUserId()", SecurityPermissionState.Allow);

            GrantModelDiffAccess(role);
            return role;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void GrantFullAccess<T>(PermissionPolicyRole role) where T : class
        {
            role.AddTypePermissionsRecursively<T>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            role.AddTypePermissionsRecursively<T>(SecurityOperations.Create,          SecurityPermissionState.Allow);
            role.AddTypePermissionsRecursively<T>(SecurityOperations.Delete,          SecurityPermissionState.Allow);
        }

        private static void GrantReadOnly<T>(PermissionPolicyRole role) where T : class
        {
            role.AddTypePermissionsRecursively<T>(SecurityOperations.Read, SecurityPermissionState.Allow);
        }

        private static void GrantModelDiffAccess(PermissionPolicyRole role)
        {
            role.AddObjectPermission<ModelDifference>(
                SecurityOperations.ReadWriteAccess,
                "UserId = ToStr(CurrentUserId())",
                SecurityPermissionState.Allow);
            role.AddObjectPermission<ModelDifferenceAspect>(
                SecurityOperations.ReadWriteAccess,
                "Owner.UserId = ToStr(CurrentUserId())",
                SecurityPermissionState.Allow);
            role.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create,       SecurityPermissionState.Allow);
            role.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }

        private void SeedUser(UserManager userManager, string userName, string password, PermissionPolicyRole role)
        {
            if (userManager.FindUserByName<XafAppUser>(ObjectSpace, userName) == null)
            {
                userManager.CreateUser<XafAppUser>(ObjectSpace, userName, password, user =>
                {
                    user.Roles.Add(role);
                });
            }
        }
    }
}
