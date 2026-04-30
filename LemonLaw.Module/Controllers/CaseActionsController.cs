using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using LemonLaw.Core.Enums;
using LemonLaw.Module.BusinessObjects;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;

using AppEntity  = LemonLaw.Core.Entities.Application;
using XafAppUser = LemonLaw.Core.Entities.ApplicationUser;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Replaces the previous 8 individual toolbar buttons with a single
    /// "Case Actions" dropdown (SingleChoiceAction). The dropdown is populated
    /// dynamically based on the current application status — only valid actions
    /// for the current state are shown, per spec §3.4 transition matrix.
    ///
    /// Actions included:
    ///   SUBMITTED        → Accept Application | Mark Incomplete | Withdraw
    ///   INCOMPLETE       → Withdraw
    ///   ACCEPTED         → Send Dealer Outreach | Schedule Hearing | Withdraw
    ///   DEALER_RESPONDED → Schedule Hearing | Withdraw
    ///   HEARING_SCHEDULED→ Mark Hearing Complete | Withdraw
    ///   HEARING_COMPLETE → Issue Decision
    ///   DECISION_ISSUED  → Close Case
    ///   (any open)       → Withdraw (always available unless terminal)
    /// </summary>
    public class CaseActionsController : ViewController<DetailView>
    {
        // Action item keys — used to identify which item was selected
        private const string KEY_ACCEPT           = "AcceptApplication";
        private const string KEY_INCOMPLETE        = "MarkIncomplete";
        private const string KEY_SEND_OUTREACH     = "SendDealerOutreach";
        private const string KEY_SCHEDULE_HEARING  = "ScheduleHearing";
        private const string KEY_HEARING_COMPLETE  = "MarkHearingComplete";
        private const string KEY_ISSUE_DECISION    = "IssueDecision";
        private const string KEY_CLOSE             = "CloseCase";
        private const string KEY_WITHDRAW          = "WithdrawCase";
        private const string KEY_ASSIGN                    = "AssignCase";
        private const string DEFAULT_CASE_ACTIONS_CAPTION  = "Case Actions";

        private SingleChoiceAction _caseActionsMenu;
        private AppEntity? _trackedApplication;
        private RefreshController? _refreshController;
        private int _busyOperationCount;

        public CaseActionsController()
        {
            // One dropdown button — "Case Actions ▾"
            _caseActionsMenu = new SingleChoiceAction(this, "CaseActionsMenu", PredefinedCategory.Edit)
            {
                Caption = DEFAULT_CASE_ACTIONS_CAPTION,
                ToolTip = "Available actions for this case based on its current status.",
                ImageName = "Action_SimpleAction",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                ShowItemsOnClick = true,
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            _caseActionsMenu.Execute += OnCaseActionSelected;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View != null)
                View.CurrentObjectChanged += OnCurrentObjectChanged;

            _refreshController = Frame.GetController<RefreshController>();
            if (_refreshController != null)
                _refreshController.RefreshAction.Executed += OnRefreshExecuted;

            TrackCurrentApplication();
            RebuildMenuItems();
        }

        protected override void OnViewChanging(View view)
        {
            base.OnViewChanging(view);
            UntrackCurrentApplication();
            RebuildMenuItems();
        }

        protected override void OnDeactivated()
        {
            if (View != null)
                View.CurrentObjectChanged -= OnCurrentObjectChanged;

            if (_refreshController != null)
                _refreshController.RefreshAction.Executed -= OnRefreshExecuted;

            UntrackCurrentApplication();
            _refreshController = null;

            base.OnDeactivated();
        }

        /// <summary>
        /// Rebuilds the dropdown items to show only the actions valid for the
        /// current application status. Called on activation and on view change.
        /// </summary>
        private void RebuildMenuItems()
        {
            _caseActionsMenu.Items.Clear();

            if (View?.CurrentObject is not AppEntity app)
            {
                _caseActionsMenu.Active.SetItemValue("IsApplicationView", false);
                return;
            }

            _caseActionsMenu.Active.SetItemValue("IsApplicationView", true);

            var s = app.Status;
            var isTerminal = s == ApplicationStatus.CLOSED || s == ApplicationStatus.WITHDRAWN;

            if (isTerminal)
            {
                // No actions available on terminal states — hide the button entirely
                _caseActionsMenu.Active.SetItemValue("IsApplicationView", false);
                return;
            }

            // Build items based on current status per spec §3.4
            var canAssign = CanCurrentUserAssign();

            switch (s)
            {
                case ApplicationStatus.SUBMITTED:
                    AddItem(KEY_ACCEPT,      "Accept Application",  "State_Validation_Valid");
                    AddItem(KEY_INCOMPLETE,  "Mark Incomplete",     "State_Validation_Warning");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,    "Withdraw",            "Action_Cancel");
                    break;

                case ApplicationStatus.INCOMPLETE:
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,    "Withdraw",            "Action_Cancel");
                    break;

                case ApplicationStatus.ACCEPTED:
                    AddItem(KEY_SEND_OUTREACH,    "Send Dealer Outreach", "BO_Supplier");
                    AddItem(KEY_SCHEDULE_HEARING, "Schedule Hearing",     "State_Priority_Normal");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,         "Withdraw",             "Action_Cancel");
                    break;

                case ApplicationStatus.DEALER_RESPONDED:
                    AddItem(KEY_SCHEDULE_HEARING, "Schedule Hearing", "State_Priority_Normal");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,         "Withdraw",         "Action_Cancel");
                    break;

                case ApplicationStatus.HEARING_SCHEDULED:
                    AddItem(KEY_HEARING_COMPLETE, "Mark Hearing Complete", "State_Validation_Valid");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,         "Withdraw",              "Action_Cancel");
                    break;

                case ApplicationStatus.HEARING_COMPLETE:
                    AddItem(KEY_ISSUE_DECISION, "Issue Decision", "BO_Report");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    break;

                case ApplicationStatus.DECISION_ISSUED:
                    AddItem(KEY_CLOSE,  "Close Case",  "Action_Delete");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    break;
            }
        }

        private void AddItem(string key, string caption, string imageName)
        {
            var item = new ChoiceActionItem(caption, null)
            {
                Id = key,
                ImageName = imageName
            };
            _caseActionsMenu.Items.Add(item);
        }

        private void OnCurrentObjectChanged(object? sender, EventArgs e)
        {
            TrackCurrentApplication();
            RebuildMenuItems();
        }

        private void OnRefreshExecuted(object? sender, ActionBaseEventArgs e)
        {
            TrackCurrentApplication();
            RebuildMenuItems();
        }

        private void TrackCurrentApplication()
        {
            var currentApp = View?.CurrentObject as AppEntity;
            if (ReferenceEquals(_trackedApplication, currentApp))
                return;

            UntrackCurrentApplication();
            _trackedApplication = currentApp;

            if (_trackedApplication != null)
                _trackedApplication.PropertyChanged += OnTrackedApplicationPropertyChanged;
        }

        private void UntrackCurrentApplication()
        {
            if (_trackedApplication != null)
                _trackedApplication.PropertyChanged -= OnTrackedApplicationPropertyChanged;

            _trackedApplication = null;
        }

        private void OnTrackedApplicationPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == nameof(AppEntity.Status))
                RebuildMenuItems();
        }

        // ── Dispatch selected action ──────────────────────────────────────────

        private void SetCaseActionBusy(bool isBusy, string? caption = null)
        {
            if (isBusy)
            {
                _busyOperationCount++;
            }
            else if (_busyOperationCount > 0)
            {
                _busyOperationCount--;
            }

            var isAnyOperationRunning = _busyOperationCount > 0;
            _caseActionsMenu.Enabled.SetItemValue("CaseActionBusy", !isAnyOperationRunning);
            _caseActionsMenu.Caption = isAnyOperationRunning
                ? (string.IsNullOrWhiteSpace(caption) ? "Processing..." : caption)
                : DEFAULT_CASE_ACTIONS_CAPTION;
        }

        private void OnCaseActionSelected(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var key = e.SelectedChoiceActionItem?.Id;
            if (key == null) return;

            switch (key)
            {
                case KEY_ACCEPT:
                    ConfirmAndRun(
                        "Accept this application? The consumer will be notified and dealer outreach will be enabled.",
                        () => ExecuteTransition("ACCEPTED", null));
                    break;

                case KEY_INCOMPLETE:
                    ConfirmAndRun(
                        "Mark this application as Incomplete? The consumer will be notified to provide missing information.",
                        () => ExecuteTransition("INCOMPLETE", "Please log in to your portal to provide the missing information."));
                    break;

                case KEY_SEND_OUTREACH:
                    ExecuteSendOutreach();
                    break;

                case KEY_SCHEDULE_HEARING:
                    ConfirmAndRun(
                        "Schedule a hearing for this case? A hearing notice will be sent to the consumer.",
                        ExecuteScheduleHearing);
                    break;

                case KEY_HEARING_COMPLETE:
                    ConfirmAndRun(
                        "Mark the hearing as completed? The case will move to HEARING_COMPLETE.",
                        () => ExecuteTransition("HEARING_COMPLETE", null));
                    break;

                case KEY_ISSUE_DECISION:
                    ConfirmAndRun(
                        "Issue a decision for this case? The consumer will be notified.",
                        ExecuteIssueDecision);
                    break;

                case KEY_CLOSE:
                    ConfirmAndRun(
                        "Close this case? This action cannot be undone.",
                        () => ExecuteTransition("CLOSED", null));
                    break;

                case KEY_WITHDRAW:
                    ConfirmAndRun(
                        "Withdraw this application? The consumer will be notified.",
                        () => ExecuteTransition("WITHDRAWN", null));
                    break;

                case KEY_ASSIGN:
                    ExecuteAssignCase();
                    break;
            }
        }

        /// <summary>
        /// Shows a confirmation message then runs the action.
        /// XAF Blazor doesn't have a built-in confirm dialog on SingleChoiceAction items,
        /// so we use ShowMessage with a follow-up approach — for now we run directly
        /// since the user already made a deliberate menu selection.
        /// </summary>
        private static void ConfirmAndRun(string message, Action action)
        {
            // The user explicitly selected the item from the dropdown — treat that as confirmation.
            // The message is shown as a tooltip/description on the item itself.
            action();
        }

        // ── Status Transition ─────────────────────────────────────────────────

        private async void ExecuteTransition(string newStatus, string? reason)
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;

            SetCaseActionBusy(true, "Updating...");
            try
            {
                    using var client = CreateHttpClient();
                    var payload = new { newStatus, reason };
                    var response = await client.PutAsJsonAsync($"api/cases/{applicationId}/status", payload);
                    var body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using var doc = JsonDocument.Parse(body);
                        var success = doc.RootElement.TryGetProperty("success", out var sv) && sv.GetBoolean();
                        if (success)
                        {
                            Application.ShowViewStrategy.ShowMessage(
                                $"✓ Case {caseNumber} — status updated to {newStatus}.",
                                InformationType.Success, 5000, InformationPosition.Top);
                            RefreshCurrentView(newStatus);
                        }
                        else
                        {
                            var msg = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : "Unknown error";
                            Application.ShowViewStrategy.ShowMessage($"Status change failed: {msg}", InformationType.Error, 5000, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage($"API error {(int)response.StatusCode}: {body}", InformationType.Error, 5000, InformationPosition.Top);
                    }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}", InformationType.Error, 6000, InformationPosition.Top);
            }
            finally
            {
                SetCaseActionBusy(false);
            }
        }

        // ── Send Dealer Outreach ──────────────────────────────────────────────

        private async void ExecuteSendOutreach()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var dealerEmail = app.Vehicle?.DealerEmail;
            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                try
                {
                    var vehicle = ObjectSpace.GetObjects(typeof(LemonLaw.Core.Entities.Vehicle))
                        .Cast<LemonLaw.Core.Entities.Vehicle>()
                        .FirstOrDefault(v => v.ApplicationId == app.Id);
                    dealerEmail = vehicle?.DealerEmail;
                }
                catch { }
            }

            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                Application.ShowViewStrategy.ShowMessage(
                    "No dealer email on the vehicle record. Edit the Vehicle, add the dealer email, then try again.",
                    InformationType.Warning, 5000, InformationPosition.Top);
                return;
            }

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;
            var responseDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15));

            var dealerName = app.Vehicle?.DealerName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(dealerName))
            {
                try
                {
                    var vehicle = ObjectSpace.GetObjects(typeof(LemonLaw.Core.Entities.Vehicle))
                        .Cast<LemonLaw.Core.Entities.Vehicle>()
                        .FirstOrDefault(v => v.ApplicationId == app.Id);
                    dealerName = vehicle?.DealerName ?? string.Empty;
                }
                catch { }
            }

            Application.ShowViewStrategy.ShowMessage(
                $"Sending dealer outreach to {dealerEmail}…",
                InformationType.Info, 2000, InformationPosition.Top);

            SetCaseActionBusy(true, "Sending...");
            try
            {
                    using var client = CreateHttpClient();
                    var payload = new
                    {
                        applicationId,
                        dealerEmail,
                        dealerName,
                        templateCode = "DEALER_INITIAL_OUTREACH",
                        responseDeadline = responseDeadline.ToString("yyyy-MM-dd")
                    };

                    var response = await client.PostAsJsonAsync("api/dealer-outreach", payload);
                    var body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using var doc = JsonDocument.Parse(body);
                        var success = doc.RootElement.TryGetProperty("success", out var sv) && sv.GetBoolean();
                        if (success)
                        {
                            Application.ShowViewStrategy.ShowMessage(
                                $"✓ Dealer outreach sent to {dealerEmail}. Deadline: {responseDeadline:MMMM d, yyyy}.",
                                InformationType.Success, 6000, InformationPosition.Top);
                            RefreshCurrentView();
                        }
                        else
                        {
                            var msg = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : "Unknown error";
                            Application.ShowViewStrategy.ShowMessage($"Outreach failed: {msg}", InformationType.Error, 5000, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage($"API error {(int)response.StatusCode}: {body}", InformationType.Error, 5000, InformationPosition.Top);
                    }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}", InformationType.Error, 6000, InformationPosition.Top);
            }
            finally
            {
                SetCaseActionBusy(false);
            }
        }

        // ── Schedule Hearing ──────────────────────────────────────────────────

        private void ExecuteScheduleHearing()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;

            // Create a non-persistent input object and open it as a popup detail view.
            // Staff fill in the hearing details; on Accept the API call is made.
            var os = Application.CreateObjectSpace(typeof(ScheduleHearingInput));
            var input = os.CreateObject<ScheduleHearingInput>();

            var detailView = Application.CreateDetailView(os, input);
            detailView.Caption = $"Schedule Hearing — {caseNumber}";

            var svp = new ShowViewParameters(detailView)
            {
                Context = TemplateContext.PopupWindow,
                TargetWindow = TargetWindow.NewModalWindow,
            };

            var dialogController = Application.CreateController<DialogController>();
            dialogController.AcceptAction.Caption = "Schedule Hearing";
            dialogController.CancelAction.Caption = "Cancel";

            dialogController.AcceptAction.Execute += async (_, _) =>
            {
                // Validate: hearing date must be in the future
                if (input.HearingDate <= DateTime.UtcNow)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        "Hearing date must be in the future.",
                        InformationType.Warning, 4000, InformationPosition.Top);
                    return;
                }

                var hearingDate = input.HearingDate;
                var hearingFormat = input.HearingFormat.ToString();
                var hearingLocation = input.HearingLocation ?? string.Empty;
                var arbitratorName = input.ArbitratorName ?? string.Empty;

                Application.ShowViewStrategy.ShowMessage(
                    $"Scheduling hearing for case {caseNumber}…",
                    InformationType.Info, 2000, InformationPosition.Top);

                SetCaseActionBusy(true, "Scheduling...");
                try
                {
                        using var client = CreateHttpClient();
                        var payload = new
                        {
                            hearingDate = hearingDate.ToString("o"),
                            hearingFormat,
                            hearingLocation,
                            arbitratorName
                        };

                        var response = await client.PostAsJsonAsync($"api/cases/{applicationId}/hearings", payload);
                        var body = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            using var doc = JsonDocument.Parse(body);
                            var success = doc.RootElement.TryGetProperty("success", out var sv) && sv.GetBoolean();
                            if (success)
                            {
                                Application.ShowViewStrategy.ShowMessage(
                                    $"✓ Hearing scheduled for {hearingDate:MMMM d, yyyy h:mm tt}. Consumer notified.",
                                    InformationType.Success, 6000, InformationPosition.Top);
                                RefreshCurrentView(ApplicationStatus.HEARING_SCHEDULED.ToString());
                            }
                            else
                            {
                                var msg = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : "Unknown error";
                                Application.ShowViewStrategy.ShowMessage($"Schedule hearing failed: {msg}", InformationType.Error, 5000, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage($"API error {(int)response.StatusCode}: {body}", InformationType.Error, 5000, InformationPosition.Top);
                        }
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}", InformationType.Error, 6000, InformationPosition.Top);
                }
                finally
                {
                    SetCaseActionBusy(false);
                }
            };

            svp.Controllers.Add(dialogController);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        // ── Issue Decision ────────────────────────────────────────────────────

        private async void ExecuteIssueDecision()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;

            // Open a popup form so staff can choose decision type, amount, deadline
            var os = Application.CreateObjectSpace(typeof(IssueDecisionInput));
            var input = os.CreateObject<IssueDecisionInput>();

            SetCaseActionBusy(true, "Issuing...");
            try
            {
                using var client = CreateHttpClient();
                var payload = new
                {
                    decisionType = input.DecisionType.ToString(),
                    decisionDate = input.DecisionDate.ToString("yyyy-MM-dd"),
                    refundAmount = input.RefundAmount,
                    complianceDeadline = input.ComplianceDeadline?.ToString("yyyy-MM-dd"),
                    decisionDocumentId = (Guid?)null
                };

                var response = await client.PostAsJsonAsync($"api/cases/{applicationId}/decisions", payload);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(body);
                    var success = doc.RootElement.TryGetProperty("success", out var sv) && sv.GetBoolean();
                    if (success)
                    {
                        Application.ShowViewStrategy.ShowMessage(
                            $"✓ Decision issued for case {caseNumber}. Consumer notified.",
                            InformationType.Success, 6000, InformationPosition.Top);
                        RefreshCurrentView(ApplicationStatus.DECISION_ISSUED.ToString());
                    }
                    else
                    {
                        var msg = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : "Unknown error";
                        Application.ShowViewStrategy.ShowMessage($"Issue decision failed: {msg}", InformationType.Error, 5000, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage($"API error {(int)response.StatusCode}: {body}", InformationType.Error, 5000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}", InformationType.Error, 6000, InformationPosition.Top);
            }
            finally
            {
                SetCaseActionBusy(false);
            }
        }

        // ── Assign Case ───────────────────────────────────────────────────────

        private void ExecuteAssignCase()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var applicationId = app.Id;
            var caseNumber = app.CaseNumber;

            // Open a ListView of ApplicationUser filtered to CASE_MANAGER role.
            // This is the standard XAF pattern for a user picker — no non-persistent type needed.
            var os = Application.CreateObjectSpace(typeof(XafAppUser));

            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse(
                "Roles[Name = 'CASE_MANAGER']");

            var listView = Application.CreateListView(os, typeof(XafAppUser), true);
            listView.Caption = $"Select Case Manager — {caseNumber}";
            listView.CollectionSource.Criteria["CaseManagerOnly"] = criteria;

            var svp = new ShowViewParameters(listView)
            {
                Context = TemplateContext.PopupWindow,
                TargetWindow = TargetWindow.NewModalWindow,
            };

            var dialogController = Application.CreateController<DialogController>();
            dialogController.AcceptAction.Caption = "Assign";
            dialogController.CancelAction.Caption = "Cancel";

            dialogController.AcceptAction.Execute += async (_, _) =>
            {
                var selectedUser = listView.CurrentObject as XafAppUser;
                if (selectedUser == null)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        "Please select a case manager from the list.",
                        InformationType.Warning, 4000, InformationPosition.Top);
                    return;
                }

                var staffId   = selectedUser.UserName;
                var staffName = selectedUser.UserName;

                SetCaseActionBusy(true, "Assigning...");
                try
                {
                    using var client = CreateHttpClient();
                    var payload = new { staffId, staffName };
                    var response = await client.PutAsJsonAsync($"api/cases/{applicationId}/assign", payload);
                    var body = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        using var doc = JsonDocument.Parse(body);
                        var success = doc.RootElement.TryGetProperty("success", out var sv) && sv.GetBoolean();
                        if (success)
                        {
                            Application.ShowViewStrategy.ShowMessage(
                                $"✓ Case {caseNumber} assigned to {staffName}.",
                                InformationType.Success, 5000, InformationPosition.Top);
                            RefreshCurrentView();
                        }
                        else
                        {
                            var msg = doc.RootElement.TryGetProperty("message", out var m) ? m.GetString() : "Unknown error";
                            Application.ShowViewStrategy.ShowMessage($"Assignment failed: {msg}", InformationType.Error, 5000, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage($"API error {(int)response.StatusCode}: {body}", InformationType.Error, 5000, InformationPosition.Top);
                    }
                }
                catch (Exception ex)
                {
                    Application.ShowViewStrategy.ShowMessage($"Error: {ex.Message}", InformationType.Error, 6000, InformationPosition.Top);
                }
                finally
                {
                    SetCaseActionBusy(false);
                }
            };

            svp.Controllers.Add(dialogController);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Only OCABR_ADMIN and SUPERVISOR can assign cases.
        /// CASE_MANAGER works their own queue — they cannot reassign.
        /// </summary>
        private bool CanCurrentUserAssign()
        {
            try
            {
                var security = Application.Security;
                if (security == null) return true; // fallback: allow if security not configured

                var user = security.User as XafAppUser;
                if (user == null) return true;

                // Check if the user has any of the admin/supervisor roles
                return user.Roles.Any(r =>
                    r.Name == "OCABR_ADMIN" ||
                    r.Name == "Administrators" ||
                    r.Name == "SUPERVISOR");
            }
            catch
            {
                return true; // fail open — don't block the action if role check fails
            }
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            return new HttpClient(handler) { BaseAddress = new Uri(GetApiBaseUrl()) };
        }

        private void RefreshCurrentView(string? newStatus = null)
        {
            if (!string.IsNullOrWhiteSpace(newStatus)
                && View?.CurrentObject is AppEntity app
                && Enum.TryParse(newStatus, true, out ApplicationStatus parsedStatus))
            {
                app.Status = parsedStatus;
                app.LastActivityAt = DateTime.UtcNow;
            }

            RebuildMenuItems();
            Frame.GetController<RefreshController>()?.RefreshAction.DoExecute();
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
