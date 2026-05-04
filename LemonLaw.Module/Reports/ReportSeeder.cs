using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using System.Text;

namespace LemonLaw.Module.Reports
{
    /// <summary>
    /// Seeds pre-built report templates into the ReportDataV2 table.
    ///
    /// Pattern (same as FILIR):
    ///   1. Load the .repx layout file from the output directory
    ///   2. Attach StaticListLookUpSettings to enum parameters in code
    ///      (more reliable than embedding lookup XML in the .repx file)
    ///   3. Serialize the configured report to bytes and store in ReportDataV2
    ///
    /// Idempotent — skips any report that already exists by DisplayName.
    /// To re-seed: DELETE FROM ReportDataV2 and restart.
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

        private static void SeedReport(IObjectSpace objectSpace, string displayName, string fileName)
        {
            if (objectSpace.GetObjectsQuery<ReportDataV2>().Any(r => r.DisplayName == displayName))
                return;

            var path = Path.Combine(AppContext.BaseDirectory, "Reports", fileName);
            if (!File.Exists(path))
            {
                Console.WriteLine($"[ReportSeeder] File not found, skipping: {path}");
                return;
            }

            // Load the report layout
            var xtraReport = new XtraReport();
            xtraReport.LoadLayoutFromXml(path);

            // Attach dropdown lookup settings to enum parameters
            AttachLookups(xtraReport, displayName);

            // Serialize the configured report to bytes
            using var stream = new MemoryStream();
            xtraReport.SaveLayoutToXml(stream);

            var record = objectSpace.CreateObject<ReportDataV2>();
            record.DisplayName = displayName;
            record.Content     = stream.ToArray();
        }

        // ── Lookup definitions ────────────────────────────────────────────────

        private static void AttachLookups(XtraReport report, string displayName)
        {
            switch (displayName)
            {
                case "Applications":
                    SetStaticLookup(report, "pStatus", new[]
                    {
                        ("(All)",              ""),
                        ("Submitted",          "SUBMITTED"),
                        ("Incomplete",         "INCOMPLETE"),
                        ("Accepted",           "ACCEPTED"),
                        ("Dealer Responded",   "DEALER_RESPONDED"),
                        ("Hearing Scheduled",  "HEARING_SCHEDULED"),
                        ("Hearing Complete",   "HEARING_COMPLETE"),
                        ("Decision Issued",    "DECISION_ISSUED"),
                        ("Withdrawn",          "WITHDRAWN"),
                        ("Closed",             "CLOSED"),
                    });
                    SetStaticLookup(report, "pType", new[]
                    {
                        ("(All)",    ""),
                        ("New Car",  "NEW_CAR"),
                        ("Used Car", "USED_CAR"),
                        ("Leased",   "LEASED"),
                    });
                    break;

                case "Hearings":
                    SetStaticLookup(report, "pOutcome", new[]
                    {
                        ("(All)",                   ""),
                        ("Pending",                 "PENDING"),
                        ("Settled",                 "SETTLED"),
                        ("Decision for Consumer",   "DECISION_FOR_CONSUMER"),
                        ("Decision for Dealer",     "DECISION_FOR_DEALER"),
                        ("No Jurisdiction",         "NO_JURISDICTION"),
                        ("Withdrawn",               "WITHDRAWN"),
                    });
                    break;

                case "Dealer Outreach Log":
                    SetStaticLookup(report, "pStatus", new[]
                    {
                        ("(All)",     ""),
                        ("Pending",   "PENDING"),
                        ("Sent",      "SENT"),
                        ("Opened",    "OPENED"),
                        ("Responded", "RESPONDED"),
                        ("Overdue",   "OVERDUE"),
                        ("Closed",    "CLOSED"),
                    });
                    break;

                case "Decisions":
                    SetStaticLookup(report, "pType", new[]
                    {
                        ("(All)",                  ""),
                        ("Refund Ordered",         "REFUND_ORDERED"),
                        ("Replacement Ordered",    "REPLACEMENT_ORDERED"),
                        ("Reimbursement Ordered",  "REIMBURSEMENT_ORDERED"),
                        ("Claim Denied",           "CLAIM_DENIED"),
                        ("Settled Prior",          "SETTLED_PRIOR"),
                        ("Withdrawn",              "WITHDRAWN"),
                    });
                    break;
            }
        }

        private static void SetStaticLookup(XtraReport report, string paramName, (string Description, string Value)[] values)
        {
            var param = report.Parameters[paramName];
            if (param == null) return;

            var settings = new StaticListLookUpSettings();
            foreach (var (desc, val) in values)
                settings.LookUpValues.Add(new LookUpValue(val, desc));

            param.ValueSourceSettings = settings;
        }
    }
}
