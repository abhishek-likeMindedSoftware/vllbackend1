using LemonLaw.Core.Entities;
using LemonLaw.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(LemonLawAPIDbContext context, ILogger logger)
    {
        logger.LogInformation("=== DatabaseSeeder.SeedAsync STARTED ===");
        
        // Schema is owned by LemonLawDbContext (XAF) — no migrations run here.
        // Just seed reference data if not already present.
        if (!await context.CorrespondenceTemplates.AnyAsync())
        {
            logger.LogInformation("Seeding correspondence templates...");
            await SeedTemplatesAsync(context);
            await context.SaveChangesAsync();
            logger.LogInformation("Correspondence templates seeded.");
        }
        else
        {
            logger.LogInformation("Correspondence templates already exist, skipping.");
        }

        // Seed one new test application each time for development/testing
        try
        {
            logger.LogInformation("=== Starting test application seeding ===");
            await SeedOneTestApplicationAsync(context, logger);
            logger.LogInformation("=== Test application seeding completed ===");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to seed test application. Error: {Message}", ex.Message);
            if (ex.InnerException != null)
            {
                logger.LogError(ex.InnerException, "Inner exception: {InnerMessage}", ex.InnerException.Message);
            }
        }
        
        logger.LogInformation("=== DatabaseSeeder.SeedAsync COMPLETED ===");
    }

    private static async Task SeedTemplatesAsync(LemonLawAPIDbContext context)
    {
        var templates = new List<CorrespondenceTemplate>
        {
            new()
            {
                TemplateCode = "CONSUMER_EMAIL_VERIFICATION",
                TemplateName = "Consumer Email Verification",
                Subject = "Your Lemon Law Application Verification Code",
                BodyHtml = """
                    <p>Dear Applicant,</p>
                    <p>Your verification code for your Massachusetts Lemon Law application is:</p>
                    <h2 style="letter-spacing:4px;">{{verificationCode}}</h2>
                    <p>This code expires in 15 minutes. Do not share it with anyone.</p>
                    <p>If you did not request this code, please ignore this email.</p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Your verification code is: {{verificationCode}}. This code expires in 15 minutes.",
                MergeFields = "[\"verificationCode\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_SUBMISSION_CONFIRMATION",
                TemplateName = "Consumer Submission Confirmation",
                Subject = "Lemon Law Application Received — Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>Your <strong>{{applicationTypeFriendly}}</strong> application has been received by the Office of Consumer Affairs and Business Regulation (OCABR).</p>
                    <p><strong>Case Number:</strong> {{caseNumber}}</p>
                    <p>You can check the status of your application at any time using the secure link below:</p>
                    <p><a href="{{portalStatusLink}}">View Application Status</a></p>
                    <p>Please save this email. The link above is your only way to access your application.</p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Your {{applicationTypeFriendly}} application (Case {{caseNumber}}) has been received. Track your status at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"applicationTypeFriendly\",\"caseNumber\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_APPLICATION_INCOMPLETE",
                TemplateName = "Consumer Application Incomplete",
                Subject = "Action Required — Your Lemon Law Application {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>OCABR staff have reviewed your application (Case {{caseNumber}}) and found that additional information or documents are needed before it can be processed.</p>
                    <p><strong>Missing items:</strong></p>
                    <p>{{missingDocumentsList}}</p>
                    <p>Please log in to your application portal to provide the requested information:</p>
                    <p><a href="{{portalStatusLink}}">Access Your Application</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Your application {{caseNumber}} requires additional information. Please visit: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"missingDocumentsList\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_APPLICATION_ACCEPTED",
                TemplateName = "Consumer Application Accepted",
                Subject = "Your Lemon Law Application Has Been Accepted — Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>Your <strong>{{applicationTypeFriendly}}</strong> application (Case {{caseNumber}}) has been accepted by OCABR. We have notified the dealer/manufacturer and requested their response.</p>
                    <p>You will be notified when the dealer responds or when further action is required.</p>
                    <p><a href="{{portalStatusLink}}">View Application Status</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Your application {{caseNumber}} has been accepted. Track status at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"applicationTypeFriendly\",\"caseNumber\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_HEARING_SCHEDULED",
                TemplateName = "Consumer Hearing Scheduled",
                Subject = "Hearing Scheduled — Lemon Law Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>An arbitration hearing has been scheduled for your Lemon Law case ({{caseNumber}}).</p>
                    <p><strong>Date:</strong> {{hearingDate}}<br/>
                    <strong>Time:</strong> {{hearingTime}}<br/>
                    <strong>Format:</strong> {{hearingFormat}}<br/>
                    <strong>Details:</strong> {{hearingDetails}}</p>
                    <p>Please ensure you are available at the scheduled time. Contact OCABR if you need to reschedule.</p>
                    <p><a href="{{portalStatusLink}}">View Application Status</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Hearing scheduled for case {{caseNumber}} on {{hearingDate}} at {{hearingTime}}. Format: {{hearingFormat}}. Details: {{hearingDetails}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"hearingDate\",\"hearingTime\",\"hearingFormat\",\"hearingDetails\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_DECISION_ISSUED",
                TemplateName = "Consumer Decision Issued",
                Subject = "Decision Issued — Lemon Law Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>A decision has been issued for your Lemon Law case ({{caseNumber}}).</p>
                    <p><strong>Decision:</strong> {{decisionTypeFriendly}}</p>
                    <p>Please log in to your application portal to view and download the full decision document:</p>
                    <p><a href="{{portalStatusLink}}">View Decision</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Decision issued for case {{caseNumber}}: {{decisionTypeFriendly}}. View at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"decisionTypeFriendly\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "DEALER_INITIAL_OUTREACH",
                TemplateName = "Dealer Initial Outreach",
                Subject = "OCABR Lemon Law Notice — Response Required by {{responseDeadline}}",
                BodyHtml = """
                    <p>Dear {{dealerName}},</p>
                    <p>The Massachusetts Office of Consumer Affairs and Business Regulation (OCABR) has received a Lemon Law application regarding a vehicle associated with your dealership.</p>
                    <p><strong>Case Number:</strong> {{caseNumber}}<br/>
                    <strong>Consumer:</strong> {{consumerName}}<br/>
                    <strong>Vehicle:</strong> {{vehicleYMM}}<br/>
                    <strong>VIN:</strong> {{vin}}</p>
                    <p>You are required to submit a formal response by <strong>{{responseDeadline}}</strong>.</p>
                    <p>Please click the link below to review the application and submit your response:</p>
                    <p><a href="{{dealerPortalLink}}">Submit Dealer Response</a></p>
                    <p>This link is unique to your case and expires on {{responseDeadline}}. Do not share it.</p>
                    <p>For questions, contact OCABR at {{ocabrContactEmail}} or {{ocabrContactPhone}}.</p>
                    <p>— {{ocabrContactName}}<br/>OCABR Lemon Law Program</p>
                    """,
                BodyText = "OCABR Lemon Law Notice for case {{caseNumber}}. Response required by {{responseDeadline}}. Submit at: {{dealerPortalLink}}",
                MergeFields = "[\"dealerName\",\"caseNumber\",\"consumerName\",\"vehicleYMM\",\"vin\",\"responseDeadline\",\"dealerPortalLink\",\"ocabrContactName\",\"ocabrContactPhone\",\"ocabrContactEmail\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "DEALER_FOLLOW_UP_1",
                TemplateName = "Dealer Follow-Up 1",
                Subject = "REMINDER: Lemon Law Response Required — Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{dealerName}},</p>
                    <p>This is a reminder that OCABR has not yet received your response for Lemon Law case <strong>{{caseNumber}}</strong>.</p>
                    <p>Your response was due by <strong>{{responseDeadline}}</strong>. Please submit your response immediately using the link below:</p>
                    <p><a href="{{dealerPortalLink}}">Submit Dealer Response</a></p>
                    <p>Failure to respond may result in the case proceeding to arbitration without your input.</p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "REMINDER: Response required for case {{caseNumber}}. Submit at: {{dealerPortalLink}}",
                MergeFields = "[\"dealerName\",\"caseNumber\",\"responseDeadline\",\"dealerPortalLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "DEALER_FOLLOW_UP_2",
                TemplateName = "Dealer Follow-Up 2",
                Subject = "SECOND NOTICE: Lemon Law Response Required — Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{dealerName}},</p>
                    <p>This is a second and final reminder. OCABR has not received your response for Lemon Law case <strong>{{caseNumber}}</strong>.</p>
                    <p>Please submit your response immediately:</p>
                    <p><a href="{{dealerPortalLink}}">Submit Dealer Response</a></p>
                    <p>If no response is received, this case will be escalated to arbitration.</p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "SECOND NOTICE: Response required for case {{caseNumber}}. Submit at: {{dealerPortalLink}}",
                MergeFields = "[\"dealerName\",\"caseNumber\",\"responseDeadline\",\"dealerPortalLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "DEALER_FINAL_NOTICE",
                TemplateName = "Dealer Final Notice",
                Subject = "FINAL NOTICE: Lemon Law Case {{caseNumber}} — Escalation Pending",
                BodyHtml = """
                    <p>Dear {{dealerName}},</p>
                    <p>OCABR has not received a response for Lemon Law case <strong>{{caseNumber}}</strong> despite multiple notices.</p>
                    <p>This is your final opportunity to submit a response before this case is escalated to arbitration:</p>
                    <p><a href="{{dealerPortalLink}}">Submit Dealer Response</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "FINAL NOTICE for case {{caseNumber}}. Submit response at: {{dealerPortalLink}} or case will be escalated.",
                MergeFields = "[\"dealerName\",\"caseNumber\",\"responseDeadline\",\"dealerPortalLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_DEALER_RESPONDED",
                TemplateName = "Consumer — Dealer Response Received",
                Subject = "Dealer Response Received — Lemon Law Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>The dealer has submitted a formal response to your Lemon Law case ({{caseNumber}}).</p>
                    <p>OCABR staff will review the response and contact you with next steps. You can check your case status at any time using the link below:</p>
                    <p><a href="{{portalStatusLink}}">View Application Status</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "The dealer has responded to your Lemon Law case {{caseNumber}}. View status at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_CASE_CLOSED",
                TemplateName = "Consumer Case Closed",
                Subject = "Your Lemon Law Case Has Been Closed — Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>Your Lemon Law case ({{caseNumber}}) has been closed.</p>
                    <p>If you have questions about this decision, please contact OCABR.</p>
                    <p><a href="{{portalStatusLink}}">View Case Details</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Your Lemon Law case {{caseNumber}} has been closed. View details at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_DOCUMENT_REJECTED",
                TemplateName = "Consumer Document Rejected",
                Subject = "Document Rejected — Lemon Law Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>A document you submitted for case {{caseNumber}} has been rejected.</p>
                    <p><strong>Reason:</strong> {{rejectionReason}}</p>
                    <p>Please log in to your portal to resubmit the document:</p>
                    <p><a href="{{portalStatusLink}}">Access Your Application</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "A document for case {{caseNumber}} was rejected. Reason: {{rejectionReason}}. Resubmit at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"rejectionReason\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            },
            new()
            {
                TemplateCode = "CONSUMER_DOCUMENT_REQUESTED",
                TemplateName = "Consumer Document Requested",
                Subject = "Document Requested — Lemon Law Case {{caseNumber}}",
                BodyHtml = """
                    <p>Dear {{consumerName}},</p>
                    <p>OCABR is requesting an additional document for your case ({{caseNumber}}).</p>
                    <p><strong>Document needed:</strong> {{documentDescription}}</p>
                    <p>Please log in to your portal to upload the requested document:</p>
                    <p><a href="{{portalStatusLink}}">Access Your Application</a></p>
                    <p>— OCABR Lemon Law Program</p>
                    """,
                BodyText = "Document requested for case {{caseNumber}}: {{documentDescription}}. Upload at: {{portalStatusLink}}",
                MergeFields = "[\"consumerName\",\"caseNumber\",\"documentDescription\",\"portalStatusLink\"]",
                IsActive = true,
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedByName = "System"
            }
        };

        await context.CorrespondenceTemplates.AddRangeAsync(templates);
    }

    private static async Task SeedOneTestApplicationAsync(LemonLawAPIDbContext context, ILogger logger)
    {
        try
        {
            logger.LogInformation("Querying for last case number...");
            
            // Get the next case number - use IgnoreQueryFilters to see ALL records including soft-deleted
            var lastCaseNumber = await context.VllApplications
                .IgnoreQueryFilters()  // ← See all records, not just non-deleted ones
                .Where(a => a.CaseNumber.StartsWith("LL-2026-"))
                .OrderByDescending(a => a.CaseNumber)
                .Select(a => a.CaseNumber)
                .FirstOrDefaultAsync();

            logger.LogInformation("Last case number found: {LastCaseNumber}", lastCaseNumber ?? "None");

            int nextNumber = 1001;
            if (!string.IsNullOrEmpty(lastCaseNumber))
            {
                var numberPart = lastCaseNumber.Split('-').Last();
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }
            
            var newCaseNumber = $"LL-2026-{nextNumber:D5}";
            logger.LogInformation("Next case number will be: {CaseNumber}", newCaseNumber);

            var random = new Random();
            var makes = new[] { "Toyota", "Honda", "Ford", "Chevrolet", "Nissan", "BMW", "Mercedes-Benz", "Volkswagen", "Hyundai", "Kia" };
            var models = new Dictionary<string, string[]>
            {
                ["Toyota"] = new[] { "Camry", "Corolla", "RAV4", "Highlander", "Tacoma" },
                ["Honda"] = new[] { "Civic", "Accord", "CR-V", "Pilot", "Odyssey" },
                ["Ford"] = new[] { "F-150", "Escape", "Explorer", "Mustang", "Edge" },
                ["Chevrolet"] = new[] { "Silverado", "Equinox", "Malibu", "Traverse", "Tahoe" },
                ["Nissan"] = new[] { "Altima", "Rogue", "Sentra", "Pathfinder", "Murano" },
                ["BMW"] = new[] { "3 Series", "5 Series", "X3", "X5", "7 Series" },
                ["Mercedes-Benz"] = new[] { "C-Class", "E-Class", "GLC", "GLE", "S-Class" },
                ["Volkswagen"] = new[] { "Jetta", "Passat", "Tiguan", "Atlas", "Golf" },
                ["Hyundai"] = new[] { "Elantra", "Sonata", "Tucson", "Santa Fe", "Palisade" },
                ["Kia"] = new[] { "Forte", "Optima", "Sportage", "Sorento", "Telluride" }
            };

            var defectCategories = new[] { 
                (Name: "Engine", Enum: Core.Enums.DefectCategory.ENGINE),
                (Name: "Transmission", Enum: Core.Enums.DefectCategory.TRANSMISSION),
                (Name: "Electrical", Enum: Core.Enums.DefectCategory.ELECTRICAL),
                (Name: "Brakes", Enum: Core.Enums.DefectCategory.BRAKES),
                (Name: "Steering", Enum: Core.Enums.DefectCategory.STEERING)
            };
            
            var defectDescriptions = new Dictionary<string, string[]>
            {
                ["Engine"] = new[] { "Engine stalls while driving", "Check engine light constantly on", "Engine makes loud knocking noise", "Loss of power during acceleration" },
                ["Transmission"] = new[] { "Transmission slips between gears", "Delayed shifting", "Grinding noise when shifting", "Transmission fluid leaking" },
                ["Electrical"] = new[] { "Battery drains overnight", "Dashboard lights flickering", "Power windows not working", "Infotainment system freezes" },
                ["Brakes"] = new[] { "Brake pedal goes to floor", "Grinding noise when braking", "ABS light stays on", "Brake fluid leaking" },
                ["Steering"] = new[] { "Steering wheel vibrates", "Power steering failure", "Steering pulls to one side", "Loose steering" }
            };

            var make = makes[random.Next(makes.Length)];
            var model = models[make][random.Next(models[make].Length)];
            var year = (short)(2021 + random.Next(4)); // 2021-2024
            var vin = $"1HGBH41JXMN{random.Next(100000, 999999)}";
            
            var defectCategoryTuple = defectCategories[random.Next(defectCategories.Length)];
            var defectCategoryName = defectCategoryTuple.Name;
            var defectCategoryEnum = defectCategoryTuple.Enum;
            var defectDesc = defectDescriptions[defectCategoryName][random.Next(defectDescriptions[defectCategoryName].Length)];

            logger.LogInformation("Creating test application entity for {Year} {Make} {Model}...", year, make, model);

            // Create application first WITHOUT related entities
            var application = new VllApplication
            {
                CaseNumber = newCaseNumber,
                ApplicationType = nextNumber % 3 == 0 ? Core.Enums.ApplicationType.NEW_CAR : Core.Enums.ApplicationType.USED_CAR,
                Status = Core.Enums.ApplicationStatus.SUBMITTED,
                SubmittedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                LastActivityAt = DateTime.UtcNow.AddDays(-random.Next(1, 5)),
                NarrativeStatement = $"I purchased this {year} {make} {model} and have experienced recurring issues with the {defectCategoryName.ToLower()}. Despite multiple repair attempts, the problem persists and significantly affects the vehicle's safety and usability.",
                PriorContactDealer = true,
                PriorContactMfr = nextNumber % 2 == 0,
                PriorContactNotes = "Contacted dealer multiple times. They acknowledged the issue but have been unable to fix it permanently.",
                DesiredResolution = nextNumber % 3 == 0 ? Core.Enums.DesiredResolution.REFUND : Core.Enums.DesiredResolution.REPLACEMENT
            };

            // Add application and save to generate ID
            context.VllApplications.Add(application);
            logger.LogInformation("Saving application to generate ID...");
            await context.SaveChangesAsync();
            logger.LogInformation("Application saved with ID: {ApplicationId}", application.Id);
            
            // Now create related entities with the generated ApplicationId
            logger.LogInformation("Creating related entities...");
            
            var applicant = new Applicant
            {
                ApplicationId = application.Id,
                FirstName = $"Test{nextNumber}",
                LastName = "User",
                EmailAddress = "test.vll@yopmail.com",
                PhoneNumber = $"617-555-{random.Next(1000, 9999):D4}",
                PhoneType = Core.Enums.PhoneType.MOBILE,
                AddressLine1 = $"{random.Next(100, 999)} Main Street",
                City = "Boston",
                AddressState = "MA",
                ZipCode = "02110",
                PreferredContact = Core.Enums.PreferredContactMethod.EMAIL
            };
            context.Applicants.Add(applicant);

            var vehicle = new Vehicle
            {
                ApplicationId = application.Id,
                VIN = vin,
                VehicleYear = year,
                VehicleMake = make,
                VehicleModel = model,
                VehicleColor = new[] { "Black", "White", "Silver", "Blue", "Red" }[random.Next(5)],
                LicensePlate = $"{random.Next(100, 999)}ABC",
                LicensePlateState = "MA",
                PurchaseDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-random.Next(6, 24))),
                PurchasePrice = 25000 + random.Next(5000, 30000),
                MileageAtPurchase = random.Next(10, 100),
                CurrentMileage = random.Next(1000, 15000),
                DealerName = $"{make} of Boston",
                DealerAddressLine1 = $"{random.Next(100, 999)} Commonwealth Avenue",
                DealerCity = "Boston",
                DealerState = "MA",
                DealerZip = "02215",
                DealerPhone = "617-555-0100",
                DealerEmail = "dealer.vll@yopmail.com",
                ManufacturerName = make,
                WarrantyType = Core.Enums.WarrantyType.MANUFACTURERS_WARRANTY,
                WarrantyStartDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-random.Next(6, 24))),
                WarrantyExpiryDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(random.Next(12, 36)))
            };
            context.Vehicles.Add(vehicle);

            // Add defect
            logger.LogInformation("Adding defect...");
            var defect = new Defect
            {
                ApplicationId = application.Id,
                DefectDescription = defectDesc,
                DefectCategory = defectCategoryEnum,
                FirstOccurrenceDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-random.Next(3, 12))),
                IsOngoing = true,
                SortOrder = 1
            };
            context.Defects.Add(defect);

            // Add 2-3 repair attempts
            var repairCount = random.Next(2, 4);
            logger.LogInformation("Adding {RepairCount} repair attempts...", repairCount);
            for (int r = 1; r <= repairCount; r++)
            {
                var repair = new RepairAttempt
                {
                    ApplicationId = application.Id,
                    RepairDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-random.Next(1, 10))),
                    RepairFacilityName = $"{make} Service Center",
                    RepairFacilityAddr = $"{random.Next(100, 999)} Service Road, Boston, MA 02215",
                    RoNumber = $"RO-{random.Next(10000, 99999)}",
                    MileageAtRepair = random.Next(500, 10000),
                    DefectsAddressed = defectDesc,
                    RepairSuccessful = false,
                    DaysOutOfService = random.Next(1, 5),
                    SortOrder = r
                };
                context.RepairAttempts.Add(repair);
            }

            // Add 1-2 expenses
            var expenseCount = random.Next(1, 3);
            logger.LogInformation("Adding {ExpenseCount} expenses...", expenseCount);
            for (int e = 1; e <= expenseCount; e++)
            {
                var expense = new Expense
                {
                    ApplicationId = application.Id,
                    ExpenseType = e == 1 ? Core.Enums.ExpenseType.RENTAL_CAR : Core.Enums.ExpenseType.TOWING,
                    ExpenseDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(-random.Next(1, 8))),
                    Amount = e == 1 ? random.Next(200, 500) : random.Next(75, 150),
                    Description = e == 1 ? "Rental car while vehicle in shop" : "Towing to repair facility",
                    ReceiptUploaded = false
                };
                context.Expenses.Add(expense);
            }

            logger.LogInformation("Saving all related entities...");
            await context.SaveChangesAsync();
            logger.LogInformation("Test application {CaseNumber} and all related entities saved successfully", newCaseNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in SeedOneTestApplicationAsync: {Message}", ex.Message);
            if (ex.InnerException != null)
            {
                logger.LogError(ex.InnerException, "Inner exception in SeedOneTestApplicationAsync: {InnerMessage}", ex.InnerException.Message);
            }
            throw; // Re-throw to be caught by outer try-catch
        }
    }
}
