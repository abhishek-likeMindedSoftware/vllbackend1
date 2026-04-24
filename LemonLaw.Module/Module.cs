using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Entities.Faq;
using System.ComponentModel;
using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Module
{
    public sealed class LemonLawModule : ModuleBase
    {
        public LemonLawModule()
        {
            // ── XAF framework types ───────────────────────────────────────────
            AdditionalExportedTypes.Add(typeof(ApplicationUser));
            AdditionalExportedTypes.Add(typeof(ApplicationUserLoginInfo));
            AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.PermissionPolicy.PermissionPolicyRole));
            AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifference));
            AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ModelDifferenceAspect));
            AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.DashboardData));
            AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.EF.ReportDataV2));

            // ── Lemon Law business entities ───────────────────────────────────
            AdditionalExportedTypes.Add(typeof(AppEntity));
            AdditionalExportedTypes.Add(typeof(Applicant));
            AdditionalExportedTypes.Add(typeof(Vehicle));
            AdditionalExportedTypes.Add(typeof(Defect));
            AdditionalExportedTypes.Add(typeof(RepairAttempt));
            AdditionalExportedTypes.Add(typeof(Expense));
            AdditionalExportedTypes.Add(typeof(ApplicationDocument));
            AdditionalExportedTypes.Add(typeof(ApplicationToken));
            AdditionalExportedTypes.Add(typeof(CaseEvent));
            AdditionalExportedTypes.Add(typeof(CaseNote));
            AdditionalExportedTypes.Add(typeof(Correspondence));
            AdditionalExportedTypes.Add(typeof(CorrespondenceTemplate));
            AdditionalExportedTypes.Add(typeof(DealerOutreach));
            AdditionalExportedTypes.Add(typeof(DealerResponse));
            AdditionalExportedTypes.Add(typeof(Hearing));
            AdditionalExportedTypes.Add(typeof(Decision));
            AdditionalExportedTypes.Add(typeof(FaqQuestion));
            AdditionalExportedTypes.Add(typeof(FaqAnswer));
            // Non-persistent dashboard host entity
            AdditionalExportedTypes.Add(typeof(AdminDashboard));

            // ── Required modules ──────────────────────────────────────────────
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.AuditTrail.EFCore.AuditTrailModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
            RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));

            DevExpress.ExpressApp.Security.SecurityModule.UsedExportedTypes =
                DevExpress.Persistent.Base.UsedExportedTypes.Custom;
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);

            // Register all business entities with XAF's type system.
            // This is required for XAF ORM to fully resolve relationships,
            // lazy loading, and detail view navigation.
            application.TypesInfo.RegisterEntity(typeof(AppEntity));
            application.TypesInfo.RegisterEntity(typeof(Applicant));
            application.TypesInfo.RegisterEntity(typeof(Vehicle));
            application.TypesInfo.RegisterEntity(typeof(Defect));
            application.TypesInfo.RegisterEntity(typeof(RepairAttempt));
            application.TypesInfo.RegisterEntity(typeof(Expense));
            application.TypesInfo.RegisterEntity(typeof(ApplicationDocument));
            application.TypesInfo.RegisterEntity(typeof(ApplicationToken));
            application.TypesInfo.RegisterEntity(typeof(CaseEvent));
            application.TypesInfo.RegisterEntity(typeof(CaseNote));
            application.TypesInfo.RegisterEntity(typeof(Correspondence));
            application.TypesInfo.RegisterEntity(typeof(CorrespondenceTemplate));
            application.TypesInfo.RegisterEntity(typeof(DealerOutreach));
            application.TypesInfo.RegisterEntity(typeof(DealerResponse));
            application.TypesInfo.RegisterEntity(typeof(Hearing));
            application.TypesInfo.RegisterEntity(typeof(Decision));
            application.TypesInfo.RegisterEntity(typeof(FaqQuestion));
            application.TypesInfo.RegisterEntity(typeof(FaqAnswer));
        }

        public override void Setup(ApplicationModulesManager moduleManager)
        {
            base.Setup(moduleManager);
        }

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(
            IObjectSpace objectSpace, Version versionFromDB)
        {
            return new ModuleUpdater[] { new DatabaseUpdate.Updater(objectSpace, versionFromDB) };
        }
    }
}
