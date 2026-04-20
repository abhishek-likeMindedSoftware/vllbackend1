using LemonLaw.Application.Interfaces;
using LemonLaw.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LemonLaw.Infrastructure.Services;

public class SendGridEmailService(
    LemonLawAPIDbContext context,
    IConfiguration configuration,
    ILogger<SendGridEmailService> logger) : IEmailService
{
    public async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, string plainTextBody)
    {
        var apiKey = configuration["SendGrid:ApiKey"];
        var fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@example.com";
        var fromName = configuration["SendGrid:FromName"] ?? "LemonLaw";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            logger.LogWarning("SendGrid API key is not configured. Email to {Email} not sent.", toEmail);
            return;
        }

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(fromEmail, fromName),
            Subject = subject,
            HtmlContent = htmlBody,
            PlainTextContent = plainTextBody
        };
        msg.AddTo(new EmailAddress(toEmail, toName));

        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Body.ReadAsStringAsync();
            logger.LogError("SendGrid returned {StatusCode} for email to {Email}. Body: {Body}",
                response.StatusCode, toEmail, body);
        }
        else
        {
            logger.LogInformation("Email sent to {Email} with subject '{Subject}'.", toEmail, subject);
        }
    }

    public async Task SendFromTemplateAsync(
        string toEmail, string toName, string templateCode, Dictionary<string, string> mergeFields)
    {
        var template = await context.CorrespondenceTemplates
            .FirstOrDefaultAsync(t => t.TemplateCode == templateCode && t.IsActive);

        if (template == null)
        {
            logger.LogError("Correspondence template '{TemplateCode}' not found or inactive.", templateCode);
            return;
        }

        var subject = ReplaceMergeFields(template.Subject, mergeFields);
        var htmlBody = ReplaceMergeFields(template.BodyHtml, mergeFields);
        var plainBody = ReplaceMergeFields(template.BodyText, mergeFields);

        await SendAsync(toEmail, toName, subject, htmlBody, plainBody);
    }

    private static string ReplaceMergeFields(string template, Dictionary<string, string> fields)
    {
        foreach (var (key, value) in fields)
            template = template.Replace($"{{{{{key}}}}}", value);
        return template;
    }
}
