using LemonLaw.Core.Entities;
using LemonLaw.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(LemonLawAPIDbContext context, ILogger logger)
    {
        // Schema is owned by LemonLawDbContext (XAF) — no migrations run here.
        // Just seed reference data if not already present.
        if (!await context.CorrespondenceTemplates.AnyAsync())
        {
            logger.LogInformation("Seeding correspondence templates...");
            await SeedTemplatesAsync(context);
            await context.SaveChangesAsync();
            logger.LogInformation("Correspondence templates seeded.");
        }
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
            }
        };

        await context.CorrespondenceTemplates.AddRangeAsync(templates);
    }
}
