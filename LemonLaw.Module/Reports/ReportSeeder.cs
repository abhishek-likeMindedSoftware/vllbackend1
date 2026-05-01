using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;
using System.Text;

namespace LemonLaw.Module.Reports
{
    /// <summary>
    /// Seeds pre-built report templates into the ReportDataV2 table.
    /// Follows the same pattern as the FILIR project:
    ///   - Reports are defined as .repx XML files in the Reports folder
    ///   - .repx files are copied to the output directory (see .csproj)
    ///   - Each report uses CollectionDataSource bound to an XAF entity
    ///   - No SQL queries, no connection strings — XAF handles data access
    ///
    /// Idempotent: skips any report that already exists by DisplayName.
    /// To re-seed, delete the row from ReportDataV2 and restart.
    /// </summary>
    public static class ReportSeeder
    {
        private static readonly (string DisplayName, string FileName)[] Reports =
        {
            ("Applications",        "Applications.repx"),
            ("Hearings",            "Hearings.repx"),
            ("Dealer Outreach Log", "DealerOutreachLog.repx"),
            ("Decisions",           "Decisions.repx"),
        };

        public static void SeedReports(IObjectSpace objectSpace)
        {
            foreach (var (displayName, fileName) in Reports)
                SeedReport(objectSpace, displayName, fileName);
        }

        private static void SeedReport(
            IObjectSpace objectSpace,
            string displayName,
            string fileName)
        {
            // Skip if already seeded
            var existing = objectSpace
                .GetObjectsQuery<ReportDataV2>()
                .FirstOrDefault(r => r.DisplayName == displayName);

            if (existing != null) return;

            var path = Path.Combine(AppContext.BaseDirectory, "Reports", fileName);

            if (!File.Exists(path))
            {
                // Log and skip rather than throw — missing file shouldn't crash startup
                Console.WriteLine($"[ReportSeeder] Report file not found, skipping: {path}");
                return;
            }

            var xml = File.ReadAllText(path, Encoding.UTF8);

            var report = objectSpace.CreateObject<ReportDataV2>();
            report.DisplayName = displayName;
            report.Content     = new UTF8Encoding(false).GetBytes(xml);
        }
    }
}
