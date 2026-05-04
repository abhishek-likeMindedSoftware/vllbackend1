using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.Office;
using DocumentFormat.OpenXml;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace LemonLaw.Application.Services;

public interface ILetterTemplateService
{
    Task<byte[]> GenerateHearingNoticeAsync(VllApplication application, Hearing hearing);
}

public class LetterTemplateService : ILetterTemplateService
{
    private readonly ILogger<LetterTemplateService> _logger;
    private readonly string _logoPath;

    public LetterTemplateService(ILogger<LetterTemplateService> logger)
    {
        _logger = logger;
        
        // Find logo with multiple fallback paths
        var currentDir = Directory.GetCurrentDirectory();
        var possibleLogoPaths = new[]
        {
            Path.Combine(currentDir, "wwwroot", "images", "ocabrLogo.png"),
            Path.Combine(currentDir, "..", "LemonLaw.Blazor.Server", "wwwroot", "images", "ocabrLogo.png"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "images", "ocabrLogo.png")
        };

        _logoPath = possibleLogoPaths.FirstOrDefault(File.Exists) ?? string.Empty;
        
        if (string.IsNullOrEmpty(_logoPath))
            _logger.LogWarning("OCABR logo not found in any expected location");
    }

    public async Task<byte[]> GenerateHearingNoticeAsync(VllApplication application, Hearing hearing)
    {
        try
        {
            // Load template from embedded resource
            var templateBytes = await LoadTemplateAsync("LemonLaw.Shared.Templates.Letters.HearingNotice.docx");
            
            using var templateStream = new MemoryStream(templateBytes);
            using var wordProcessor = new RichEditDocumentServer();
            
            // Load the template
            wordProcessor.LoadDocument(templateStream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
            var doc = wordProcessor.Document;

            // Replace placeholders - NOT using WholeWord for placeholders with brackets
            doc.ReplaceAll("[Current Date]", DateTime.Now.ToString("MMMM dd, yyyy"), SearchOptions.None);
            doc.ReplaceAll("[Case Number]", application.CaseNumber, SearchOptions.None);
            
            // Applicant information
            if (application.Applicant != null)
            {
                doc.ReplaceAll("[Applicant Full Name]", 
                    $"{application.Applicant.FirstName} {application.Applicant.LastName}", 
                    SearchOptions.None);
                doc.ReplaceAll("[Applicant First Name]", application.Applicant.FirstName, SearchOptions.None);
                doc.ReplaceAll("[Applicant Last Name]", application.Applicant.LastName, SearchOptions.None);
                doc.ReplaceAll("[Address Line 1]", application.Applicant.AddressLine1 ?? "", SearchOptions.None);
                doc.ReplaceAll("[City]", application.Applicant.City ?? "", SearchOptions.None);
                doc.ReplaceAll("[State]", application.Applicant.AddressState ?? "", SearchOptions.None);
                doc.ReplaceAll("[Zip Code]", application.Applicant.ZipCode ?? "", SearchOptions.None);
            }

            // Hearing information - all fields from the popup
            doc.ReplaceAll("[Hearing Date]", hearing.HearingDate.ToString("MMMM dd, yyyy"), SearchOptions.None);
            doc.ReplaceAll("[Hearing Time]", hearing.HearingDate.ToString("h:mm tt"), SearchOptions.None);
            doc.ReplaceAll("[Hearing Format]", FormatHearingFormat(hearing.HearingFormat), SearchOptions.None);
            doc.ReplaceAll("[Hearing Location]", hearing.HearingLocation ?? "TBD", SearchOptions.None);
            doc.ReplaceAll("[Arbitrator Name]", hearing.ArbitratorName ?? "TBD", SearchOptions.None);

            // Vehicle information
            if (application.Vehicle != null)
            {
                doc.ReplaceAll("[Vehicle Year]", application.Vehicle.VehicleYear.ToString(), SearchOptions.None);
                doc.ReplaceAll("[Vehicle Make]", application.Vehicle.VehicleMake, SearchOptions.None);
                doc.ReplaceAll("[Vehicle Model]", application.Vehicle.VehicleModel, SearchOptions.None);
                doc.ReplaceAll("[VIN]", application.Vehicle.VIN ?? "", SearchOptions.None);
                
                // Combined vehicle description
                doc.ReplaceAll("[Vehicle YMM]", 
                    $"{application.Vehicle.VehicleYear} {application.Vehicle.VehicleMake} {application.Vehicle.VehicleModel}", 
                    SearchOptions.None);
            }

            // Insert logo if available
            if (!string.IsNullOrEmpty(_logoPath) && File.Exists(_logoPath))
            {
                InsertLogoAtPlaceholder(doc, "[LOGO_PLACEHOLDER]", _logoPath);
            }
            else
            {
                // Remove placeholder if logo not found
                doc.ReplaceAll("[LOGO_PLACEHOLDER]", "", SearchOptions.None);
            }

            // Save to byte array
            using var outputStream = new MemoryStream();
            wordProcessor.SaveDocument(outputStream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
            return outputStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating hearing notice for application {ApplicationId}", application.Id);
            throw;
        }
    }

    private async Task<byte[]> LoadTemplateAsync(string resourceName)
    {
        try
        {
            var sharedAsm = Assembly.Load("LemonLaw.Shared");
            using var stream = sharedAsm.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Template not found: {resourceName}");
            
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load template {ResourceName}", resourceName);
            throw;
        }
    }

    private void InsertLogoAtPlaceholder(Document doc, string placeholder, string logoPath)
    {
        try
        {
            var range = doc.FindAll(placeholder, SearchOptions.None).FirstOrDefault();
            if (range == null)
            {
                _logger.LogWarning("Logo placeholder {Placeholder} not found in template", placeholder);
                return;
            }

            // Delete placeholder text
            doc.Delete(range);

            // Insert image at that position
            var image = doc.Images.Insert(doc.CreatePosition(range.Start.ToInt()), DevExpress.XtraRichEdit.API.Native.DocumentImageSource.FromFile(logoPath));
            
            // Resize image (4 inches wide, maintain aspect ratio)
            var aspectRatio = (float)image.Size.Height / image.Size.Width;
            image.Size = new System.Drawing.SizeF(
                DevExpress.Office.Utils.Units.InchesToDocumentsF(4f),
                DevExpress.Office.Utils.Units.InchesToDocumentsF(4f * aspectRatio)
            );
            
            _logger.LogInformation("Successfully inserted logo at placeholder");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to insert logo at placeholder");
            // Don't throw - letter can still be generated without logo
        }
    }

    private string FormatHearingFormat(HearingFormat format)
    {
        return format switch
        {
            HearingFormat.IN_PERSON => "In Person",
            HearingFormat.TELEPHONE => "Telephone Conference",
            HearingFormat.VIDEO => "Video Conference",
            _ => format.ToString()
        };
    }
}
