using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using System.Net.Http.Json;
using System.Text.Json;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Replaces the three individual follow-up buttons with a single
    /// "Follow-Up Actions" dropdown (SingleChoiceAction) — same pattern as
    /// CaseActionsController.
    ///
    /// Only the next valid follow-up step is shown in the dropdown.
    /// The dropdown is hidden entirely when:
    ///   - The dealer has already responded (Status == RESPONDED)
    ///   - All three follow-ups have already been sent
    ///   - The initial outreach has not been sent yet (SentAt is null)
    ///
    /// Sequence enforced per spec §4.4:
    ///   Initial sent → FOLLOW_UP_1 available
    ///   Follow-Up 1 sent → FOLLOW_UP_2 available
    ///   Follow-Up 2 sent → FINAL_NOTICE available
    ///   Final Notice sent (or dealer responded) → dropdown hidden
    /// </summary>
    public class DealerOutreachActionsController : ViewController<DetailView>
    {
        private const string KEY_FOLLOW_UP_1  = "FollowUp1";
        private const string KEY_FOLLOW_UP_2  = "FollowUp2";
        private const string KEY_FINAL_NOTICE = "FinalNotice";

        private SingleChoiceAction _followUpMenu;

        public DealerOutreachActionsController()
        {
            _followUpMenu = new SingleChoiceAction(this, "DealerFollowUpMenu", PredefinedCategory.RecordEdit)
            {
                Caption = "Follow-Up Actions",
                ToolTip = "Send the next follow-up or final notice to the dealer.",
                ImageName = "Action_Send",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                ShowItemsOnClick = true,
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            _followUpMenu.Execute += OnFollowUpSelected;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // Only show on DealerOutreach detail view
            _followUpMenu.Active.SetItemValue("IsDealerOutreachView",
                View?.CurrentObject is DealerOutreach);

            if (View?.CurrentObject is DealerOutreach)
                RebuildMenuItems();
        }

        protected override void OnViewChanging(View view)
        {
            base.OnViewChanging(view);
            RebuildMenuItems();
        }

        /// <summary>
        /// Rebuilds the dropdown to show only the next valid follow-up step.
        /// Hides the dropdown entirely when there is nothing left to send.
        /// </summary>
        private void RebuildMenuItems()
        {
            _followUpMenu.Items.Clear();

            if (View?.CurrentObject is not DealerOutreach outreach)
            {
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }

            _followUpMenu.Active.SetItemValue("IsDealerOutreachView", true);

            // Hide entirely if dealer already responded
            if (outreach.Status == OutreachStatus.RESPONDED)
            {
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }

            // Hide entirely if initial outreach hasn't been sent yet
            if (!outreach.SentAt.HasValue)
            {
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }

            // Add only the next step that hasn't been sent yet (strictly sequential)
            if (outreach.FollowUp1SentAt == null)
            {
                AddItem(KEY_FOLLOW_UP_1, "Send Follow-Up 1", "Action_Send");
            }
            else if (outreach.FollowUp2SentAt == null)
            {
                AddItem(KEY_FOLLOW_UP_2, "Send Follow-Up 2", "Action_Send");
            }
            else if (outreach.FinalNoticeSentAt == null)
            {
                AddItem(KEY_FINAL_NOTICE, "Send Final Notice", "Action_Send");
            }
            else
            {
                // All follow-ups sent — nothing left to do, hide the dropdown
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }
        }

        private void AddItem(string key, string caption, string imageName)
        {
            var item = new ChoiceActionItem(caption, null)
            {
                Id = key,
                ImageName = imageName
            };
            _followUpMenu.Items.Add(item);
        }

        // ── Dispatch ──────────────────────────────────────────────────────────

        private void OnFollowUpSelected(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var key = e.SelectedChoiceActionItem?.Id;
            if (key == null) return;

            var outreachType = key switch
            {
                KEY_FOLLOW_UP_1  => "FOLLOW_UP_1",
                KEY_FOLLOW_UP_2  => "FOLLOW_UP_2",
                KEY_FINAL_NOTICE => "FINAL_NOTICE",
                _ => null
            };

            if (outreachType != null)
                ExecuteFollowUp(outreachType);
        }

        // ── Send follow-up via API ────────────────────────────────────────────

        private void ExecuteFollowUp(string outreachType)
        {
            if (View?.CurrentObject is not DealerOutreach outreach)
                return;

            var outreachId = outreach.Id;
            var dealerEmail = outreach.DealerEmail;
            var label = outreachType switch
            {
                "FOLLOW_UP_1" => "Follow-Up 1",
                "FOLLOW_UP_2" => "Follow-Up 2",
                "FINAL_NOTICE" => "Final Notice",
                _ => outreachType
            };

            Application.ShowViewStrategy.ShowMessage(
                $"Sending {label} to {dealerEmail}…",
                InformationType.Info, 2000, InformationPosition.Top);

            _ = Task.Run(async () =>
            {
                try
                {
                    using var client = CreateHttpClient();
                    var payload = new { outreachType };
                    var response = await client.PostAsJsonAsync(
                        $"api/dealer-outreach/{outreachId}/follow-up", payload);
                    var body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using var doc = JsonDocument.Parse(body);
                        var success = doc.RootElement.TryGetProperty("success", out var s) && s.GetBoolean();

                        if (success)
                        {
                            Application.ShowViewStrategy.ShowMessage(
                                $"✓ {label} sent to {dealerEmail}.",
                                InformationType.Success, 5000, InformationPosition.Top);

                            // Refresh so timestamps update and dropdown rebuilds
                            Application.MainWindow?.GetController<RefreshController>()?.RefreshAction.DoExecute();
                        }
                        else
                        {
                            var msg = doc.RootElement.TryGetProperty("message", out var m)
                                ? m.GetString() : "Unknown error";
                            Application.ShowViewStrategy.ShowMessage(
                                $"{label} failed: {msg}",
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
                        $"Error sending {label}: {ex.Message}",
                        InformationType.Error, 6000, InformationPosition.Top);
                }
            });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            return new HttpClient(handler) { BaseAddress = new Uri(GetApiBaseUrl()) };
        }

        private string GetApiBaseUrl()
        {
            try
            {
                var config = Application.ServiceProvider?
                    .GetService(typeof(Microsoft.Extensions.Configuration.IConfiguration))
                    as Microsoft.Extensions.Configuration.IConfiguration;
                var url = config?["LemonLawApi:BaseUrl"];
                if (!string.IsNullOrWhiteSpace(url)) return url;
            }
            catch { }
            return "https://localhost:7229/";
        }
    }
}
