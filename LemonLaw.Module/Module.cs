using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Entities;
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

        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(
            IObjectSpace objectSpace, Version versionFromDB)
        {
            return new ModuleUpdater[] { new DatabaseUpdate.Updater(objectSpace, versionFromDB) };
        }

        public override void Setup(XafApplication application)
        {
            base.Setup(application);
        }

        public override void Setup(ApplicationModulesManager moduleManager)
        {
            base.Setup(moduleManager);
        }
    }
}
