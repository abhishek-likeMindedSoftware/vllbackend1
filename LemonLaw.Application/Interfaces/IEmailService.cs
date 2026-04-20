namespace LemonLaw.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(string toEmail, string toName, string subject, string htmlBody, string plainTextBody);
    Task SendFromTemplateAsync(string toEmail, string toName, string templateCode, Dictionary<string, string> mergeFields);
}
