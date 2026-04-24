using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using System.Net.Http.Json;
using System.Text.Json;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Adds a "Send Dealer Outreach" action button to the Application detail view.
    /// Calls the REST API directly via HttpClient — avoids DI resolution issues
    /// with XAF's internal service provider in Blazor Server mode.
    /// </summary>
    public class SendDealerOutreachController : ViewController<DetailView>
    {
        private SimpleAction _sendOutreachAction;

        public SendDealerOutreachController()
        {
            _sendOutreachAction = new SimpleAction(this, "SendDealerOutreach", PredefinedCategory.Edit)
            {
                Caption = "Send Dealer Outreach",
                ToolTip = "Send the initial outreach email to the dealer.",
                ImageName = "Action_Send"
            };
            _sendOutreachAction.Execute += OnSendOutreachExecute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            _sendOutreachAction.Active.SetItemValue("IsApplicationView",
                View?.CurrentObject is AppEntity);
        }

        private void OnSendOutreachExecute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View?.CurrentObject is not AppEntity app)
                return;

            if (app.Status != ApplicationStatus.ACCEPTED)
            {
                Application.ShowViewStrategy.ShowMessage(
                    $"Status must be ACCEPTED before sending outreach. Current: {app.Status}.",
                    InformationType.Warning, 4000, InformationPosition.Top);
                return;
            }

            var dealerEmail = app.Vehicle?.DealerEmail;
            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                // Vehicle may not be loaded in XAF's object space — try loading it
                try
                {
                    var vehicle = ObjectSpace.GetObjects(typeof(LemonLaw.Core.Entities.Vehicle))
                        .Cast<LemonLaw.Core.Entities.Vehicle>()
                        .FirstOrDefault(v => v.ApplicationId == app.Id);
                    dealerEmail = vehicle?.DealerEmail;
                }
                catch { /* fall through */ }
            }

            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                Application.ShowViewStrategy.ShowMessage(
                    "No dealer email on the vehicle record. " +
                    "Edit the Vehicle, add the dealer email, then try again.",
                    InformationType.Warning, 5000, InformationPosition.Top);
                return;
            }

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;
            var responseDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15));

            Application.ShowViewStrategy.ShowMessage(
                $"Sending dealer outreach to {dealerEmail}…",
                InformationType.Info, 2000, InformationPosition.Top);

            _ = Task.Run(async () =>
            {
                try
                {
                    // Read API base URL from configuration
                    var apiBase = GetApiBaseUrl();

                    using var http = new HttpClient();
                    http.BaseAddress = new Uri(apiBase);

                    // Bypass SSL validation for local dev (self-signed cert)
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                    };
                    using var client = new HttpClient(handler) { BaseAddress = new Uri(apiBase) };

                    var payload = new
                    {
                        applicationId,
                        dealerEmail,
                        templateCode = "DEALER_INITIAL_OUTREACH",
                        responseDeadline = responseDeadline.ToString("yyyy-MM-dd")
                    };

                    var response = await client.PostAsJsonAsync("api/dealer-outreach", payload);
                    var body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse success flag from response
                        using var doc = JsonDocument.Parse(body);
                        var success = doc.RootElement.TryGetProperty("success", out var s) && s.GetBoolean();

                        if (success)
                        {
                            Application.ShowViewStrategy.ShowMessage(
                                $"✓ Dealer outreach sent to {dealerEmail}. " +
                                $"Deadline: {responseDeadline:MMMM d, yyyy}.",
                                InformationType.Success, 6000, InformationPosition.Top);
                        }
                        else
                        {
                            var msg = doc.RootElement.TryGetProperty("message", out var m)
                                ? m.GetString() : "Unknown error";
                            Application.ShowViewStrategy.ShowMessage(
                                $"Outreach failed: {msg}",
                                InformationType.Error, 5000, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(
                            $"API error {(int)response.StatusCode}: {body}",
                            InformationType.Error, 5000, InformationPosition.Top);
                    }
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        $"Error: {ex.Message}",
                        InformationType.Error, 6000, InformationPosition.Top);
                }
            });
        }

        private string GetApiBaseUrl()
        {
            // Try to read from XAF application configuration
            try
            {
                var config = Application.ServiceProvider?
                    .GetService(typeof(Microsoft.Extensions.Configuration.IConfiguration))
                    as Microsoft.Extensions.Configuration.IConfiguration;
                var url = config?["LemonLawApi:BaseUrl"];
                if (!string.IsNullOrWhiteSpace(url)) return url;
            }
            catch { /* fall through to default */ }

            return "https://localhost:7229/";
        }
    }
}
