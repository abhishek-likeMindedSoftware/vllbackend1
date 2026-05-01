using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using LemonLaw.Application.Interfaces;
using LemonLaw.Core.Enums;
using LemonLaw.Module.BusinessObjects;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

using AppEntity  = LemonLaw.Core.Entities.Application;
using XafAppUser = LemonLaw.Core.Entities.ApplicationUser;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Provides the "Case Actions" dropdown on the Application detail view.
    /// All business logic is delegated directly to IApplicationService and
    /// IDealerOutreachService — no HTTP calls are made from this controller.
    ///
    /// Actions per spec §3.4 transition matrix:
    ///   SUBMITTED         → Accept | Mark Incomplete | Assign | Withdraw
    ///   INCOMPLETE        → Assign | Withdraw
    ///   ACCEPTED          → Send Dealer Outreach | Schedule Hearing | Assign | Withdraw
    ///   DEALER_RESPONDED  → Schedule Hearing | Assign | Withdraw
    ///   HEARING_SCHEDULED → Mark Hearing Complete | Assign | Withdraw
    ///   HEARING_COMPLETE  → Issue Decision | Assign
    ///   DECISION_ISSUED   → Close Case | Assign
    /// </summary>
    public class CaseActionsController : ViewController<DetailView>
    {
        private const string KEY_ACCEPT           = "AcceptApplication";
        private const string KEY_INCOMPLETE       = "MarkIncomplete";
        private const string KEY_SEND_OUTREACH    = "SendDealerOutreach";
        private const string KEY_SCHEDULE_HEARING = "ScheduleHearing";
        private const string KEY_HEARING_COMPLETE = "MarkHearingComplete";
        private const string KEY_ISSUE_DECISION   = "IssueDecision";
        private const string KEY_CLOSE            = "CloseCase";
        private const string KEY_WITHDRAW         = "WithdrawCase";
        private const string KEY_ASSIGN           = "AssignCase";

        private const string DEFAULT_CAPTION = "Case Actions";

        private readonly SingleChoiceAction _caseActionsMenu;
        private AppEntity?         _trackedApplication;
        private RefreshController? _refreshController;
        private int                _busyCount;

        public CaseActionsController()
        {
            _caseActionsMenu = new SingleChoiceAction(this, "CaseActionsMenu", PredefinedCategory.Edit)
            {
                Caption          = DEFAULT_CAPTION,
                ToolTip          = "Available actions for this case based on its current status.",
                ImageName        = "Action_SimpleAction",
                PaintStyle       = ActionItemPaintStyle.CaptionAndImage,
                ShowItemsOnClick = true,
                ItemType         = SingleChoiceActionItemType.ItemIsOperation
            };
            _caseActionsMenu.Execute += OnCaseActionSelected;
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────

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

        // ── Menu building ─────────────────────────────────────────────────────

        private void RebuildMenuItems()
        {
            _caseActionsMenu.Items.Clear();

            if (View?.CurrentObject is not AppEntity app)
            {
                _caseActionsMenu.Active.SetItemValue("IsApplicationView", false);
                return;
            }

            _caseActionsMenu.Active.SetItemValue("IsApplicationView", true);

            var status     = app.Status;
            var isTerminal = status == ApplicationStatus.CLOSED || status == ApplicationStatus.WITHDRAWN;

            if (isTerminal)
            {
                _caseActionsMenu.Active.SetItemValue("IsApplicationView", false);
                return;
            }

            var canAssign = CanCurrentUserAssign();

            switch (status)
            {
                case ApplicationStatus.SUBMITTED:
                    AddItem(KEY_ACCEPT,     "Accept Application", "State_Validation_Valid");
                    AddItem(KEY_INCOMPLETE, "Mark Incomplete",    "State_Validation_Warning");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW,   "Withdraw",           "Action_Cancel");
                    break;

                case ApplicationStatus.INCOMPLETE:
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW, "Withdraw", "Action_Cancel");
                    break;

                case ApplicationStatus.ACCEPTED:
                    AddItem(KEY_SEND_OUTREACH,    "Send Dealer Outreach", "BO_Supplier");
                    AddItem(KEY_SCHEDULE_HEARING, "Schedule Hearing",     "State_Priority_Normal");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW, "Withdraw", "Action_Cancel");
                    break;

                case ApplicationStatus.DEALER_RESPONDED:
                    AddItem(KEY_SCHEDULE_HEARING, "Schedule Hearing", "State_Priority_Normal");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW, "Withdraw", "Action_Cancel");
                    break;

                case ApplicationStatus.HEARING_SCHEDULED:
                    AddItem(KEY_HEARING_COMPLETE, "Mark Hearing Complete", "State_Validation_Valid");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    AddItem(KEY_WITHDRAW, "Withdraw", "Action_Cancel");
                    break;

                case ApplicationStatus.HEARING_COMPLETE:
                    AddItem(KEY_ISSUE_DECISION, "Issue Decision", "BO_Report");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    break;

                case ApplicationStatus.DECISION_ISSUED:
                    AddItem(KEY_CLOSE, "Close Case", "Action_Delete");
                    if (canAssign) AddItem(KEY_ASSIGN, "Assign Case", "BO_Employee");
                    break;
            }
        }

        private void AddItem(string key, string caption, string imageName) =>
            _caseActionsMenu.Items.Add(new ChoiceActionItem(caption, null) { Id = key, ImageName = imageName });

        // ── Event handlers ────────────────────────────────────────────────────

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
            var current = View?.CurrentObject as AppEntity;
            if (ReferenceEquals(_trackedApplication, current)) return;

            UntrackCurrentApplication();
            _trackedApplication = current;
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

        // ── Busy state ────────────────────────────────────────────────────────

        private void SetBusy(bool busy, string? caption = null)
        {
            _busyCount = busy ? _busyCount + 1 : Math.Max(0, _busyCount - 1);
            var running = _busyCount > 0;
            _caseActionsMenu.Enabled.SetItemValue("CaseActionBusy", !running);
            _caseActionsMenu.Caption = running
                ? (string.IsNullOrWhiteSpace(caption) ? "Processing..." : caption)
                : DEFAULT_CAPTION;
        }

        // ── Dispatch ──────────────────────────────────────────────────────────

        private void OnCaseActionSelected(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            switch (e.SelectedChoiceActionItem?.Id)
            {
                case KEY_ACCEPT:           ExecuteTransition("ACCEPTED",        null); break;
                case KEY_INCOMPLETE:       ExecuteTransition("INCOMPLETE",      "Please log in to your portal to provide the missing information."); break;
                case KEY_HEARING_COMPLETE: ExecuteTransition("HEARING_COMPLETE", null); break;
                case KEY_CLOSE:            ExecuteTransition("CLOSED",          null); break;
                case KEY_WITHDRAW:         ExecuteTransition("WITHDRAWN",       null); break;
                case KEY_SEND_OUTREACH:    ExecuteSendOutreach(); break;
                case KEY_SCHEDULE_HEARING: ExecuteScheduleHearing(); break;
                case KEY_ISSUE_DECISION:   ExecuteIssueDecision(); break;
                case KEY_ASSIGN:           ExecuteAssignCase(); break;
            }
        }

        // ── Status transition ─────────────────────────────────────────────────

        private async void ExecuteTransition(string newStatus, string? reason)
        {
            if (View?.CurrentObject is not AppEntity app) return;

            SetBusy(true, "Updating...");
            try
            {
                var svc    = GetService<IApplicationService>();
                var staffId = CurrentUserName();
                var dto    = new StatusTransitionDto { NewStatus = newStatus, Reason = reason };

                var result = await svc.TransitionStatusAsync(app.Id, dto, staffId, staffId);

                if (result.Success)
                {
                    ShowSuccess($"✓ Case {app.CaseNumber} — status updated to {newStatus}.");
                    RefreshCurrentView(newStatus);
                }
                else
                {
                    ShowError($"Status change failed: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        // ── Send dealer outreach ──────────────────────────────────────────────

        private async void ExecuteSendOutreach()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            // Resolve dealer email — prefer the loaded navigation property, fall back to ObjectSpace query
            var dealerEmail = app.Vehicle?.DealerEmail;
            var dealerName  = app.Vehicle?.DealerName ?? string.Empty;

            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                var vehicle = ObjectSpace.GetObjects(typeof(LemonLaw.Core.Entities.Vehicle))
                    .Cast<LemonLaw.Core.Entities.Vehicle>()
                    .FirstOrDefault(v => v.ApplicationId == app.Id);
                dealerEmail = vehicle?.DealerEmail;
                dealerName  = vehicle?.DealerName ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(dealerEmail))
            {
                ShowWarning("No dealer email on the vehicle record. Edit the Vehicle, add the dealer email, then try again.");
                return;
            }

            SetBusy(true, "Sending...");
            try
            {
                var svc = GetService<IDealerOutreachService>();
                var dto = new CreateOutreachDto
                {
                    ApplicationId    = app.Id,
                    DealerEmail      = dealerEmail,
                    DealerName       = dealerName,
                    TemplateCode     = "DEALER_INITIAL_OUTREACH",
                    ResponseDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15))
                };

                var result = await svc.CreateOutreachAsync(dto, CurrentUserName());

                if (result.Success)
                {
                    ShowSuccess($"✓ Dealer outreach sent to {dealerEmail}. Deadline: {dto.ResponseDeadline:MMMM d, yyyy}.");
                    RefreshCurrentView();
                }
                else
                {
                    ShowError($"Outreach failed: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
            finally
            {
                SetBusy(false);
            }
        }

        // ── Schedule hearing ──────────────────────────────────────────────────

        private void ExecuteScheduleHearing()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var os    = Application.CreateObjectSpace(typeof(ScheduleHearingInput));
            var input = os.CreateObject<ScheduleHearingInput>();

            var detailView = Application.CreateDetailView(os, input);
            detailView.Caption = $"Schedule Hearing — {app.CaseNumber}";

            var svp = new ShowViewParameters(detailView)
            {
                Context      = TemplateContext.PopupWindow,
                TargetWindow = TargetWindow.NewModalWindow
            };

            var dialog = Application.CreateController<DialogController>();
            dialog.AcceptAction.Caption = "Schedule Hearing";
            dialog.CancelAction.Caption = "Cancel";

            dialog.AcceptAction.Execute += async (_, _) =>
            {
                if (input.HearingDate <= DateTime.UtcNow)
                {
                    ShowWarning("Hearing date must be in the future.");
                    return;
                }

                SetBusy(true, "Scheduling...");
                try
                {
                    var svc = GetService<IApplicationService>();
                    var dto = new CreateHearingDto
                    {
                        HearingDate     = input.HearingDate,
                        HearingFormat   = input.HearingFormat.ToString(),
                        HearingLocation = input.HearingLocation ?? string.Empty,
                        ArbitratorName  = input.ArbitratorName  ?? string.Empty
                    };

                    var result = await svc.CreateHearingAsync(app.Id, dto, CurrentUserName());

                    if (result.Success)
                    {
                        ShowSuccess($"✓ Hearing scheduled for {input.HearingDate:MMMM d, yyyy h:mm tt}. Consumer notified.");
                        RefreshCurrentView(ApplicationStatus.HEARING_SCHEDULED.ToString());
                    }
                    else
                    {
                        ShowError($"Schedule hearing failed: {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error: {ex.Message}");
                }
                finally
                {
                    SetBusy(false);
                }
            };

            svp.Controllers.Add(dialog);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        // ── Issue decision ────────────────────────────────────────────────────

        private void ExecuteIssueDecision()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var os    = Application.CreateObjectSpace(typeof(IssueDecisionInput));
            var input = os.CreateObject<IssueDecisionInput>();

            var detailView = Application.CreateDetailView(os, input);
            detailView.Caption = $"Issue Decision — {app.CaseNumber}";

            var svp = new ShowViewParameters(detailView)
            {
                Context      = TemplateContext.PopupWindow,
                TargetWindow = TargetWindow.NewModalWindow
            };

            var dialog = Application.CreateController<DialogController>();
            dialog.AcceptAction.Caption = "Issue Decision";
            dialog.CancelAction.Caption = "Cancel";

            dialog.AcceptAction.Execute += async (_, _) =>
            {
                SetBusy(true, "Issuing...");
                try
                {
                    var svc = GetService<IApplicationService>();
                    var dto = new IssueDecisionDto
                    {
                        DecisionType       = input.DecisionType.ToString(),
                        DecisionDate       = DateOnly.FromDateTime(input.DecisionDate),
                        RefundAmount       = input.RefundAmount,
                        ComplianceDeadline = input.ComplianceDeadline.HasValue
                                                ? DateOnly.FromDateTime(input.ComplianceDeadline.Value)
                                                : null,
                        DecisionDocumentId = null
                    };

                    var result = await svc.IssueDecisionAsync(app.Id, dto, CurrentUserName());

                    if (result.Success)
                    {
                        ShowSuccess($"✓ Decision issued for case {app.CaseNumber}. Consumer notified.");
                        RefreshCurrentView(ApplicationStatus.DECISION_ISSUED.ToString());
                    }
                    else
                    {
                        ShowError($"Issue decision failed: {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error: {ex.Message}");
                }
                finally
                {
                    SetBusy(false);
                }
            };

            svp.Controllers.Add(dialog);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        // ── Assign case ───────────────────────────────────────────────────────

        private void ExecuteAssignCase()
        {
            if (View?.CurrentObject is not AppEntity app) return;

            var os       = Application.CreateObjectSpace(typeof(XafAppUser));
            var criteria = DevExpress.Data.Filtering.CriteriaOperator.Parse("Roles[Name = 'CASE_MANAGER']");

            var listView = Application.CreateListView(os, typeof(XafAppUser), true);
            listView.Caption = $"Select Case Manager — {app.CaseNumber}";
            listView.CollectionSource.Criteria["CaseManagerOnly"] = criteria;

            var svp = new ShowViewParameters(listView)
            {
                Context      = TemplateContext.PopupWindow,
                TargetWindow = TargetWindow.NewModalWindow
            };

            var dialog = Application.CreateController<DialogController>();
            dialog.AcceptAction.Caption = "Assign";
            dialog.CancelAction.Caption = "Cancel";

            dialog.AcceptAction.Execute += async (_, _) =>
            {
                if (listView.CurrentObject is not XafAppUser selectedUser)
                {
                    ShowWarning("Please select a case manager from the list.");
                    return;
                }

                SetBusy(true, "Assigning...");
                try
                {
                    var svc = GetService<IApplicationService>();
                    var dto = new AssignCaseDto
                    {
                        StaffId     = selectedUser.UserName,
                        StaffName   = selectedUser.UserName,
                        StaffUserId = selectedUser.ID
                    };

                    var result = await svc.AssignCaseAsync(app.Id, dto, CurrentUserName());

                    if (result.Success)
                    {
                        ShowSuccess($"✓ Case {app.CaseNumber} assigned to {selectedUser.UserName}.");
                        RefreshCurrentView();
                    }
                    else
                    {
                        ShowError($"Assignment failed: {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Error: {ex.Message}");
                }
                finally
                {
                    SetBusy(false);
                }
            };

            svp.Controllers.Add(dialog);
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Resolves a service from the XAF application's DI container.
        /// LemonLaw.Application services are registered in Startup.cs via AddApplication().
        /// </summary>
        private T GetService<T>() where T : notnull =>
            Application.ServiceProvider!.GetRequiredService<T>();

        private string CurrentUserName() =>
            (Application.Security?.User as XafAppUser)?.UserName ?? "Staff";

        private bool CanCurrentUserAssign()
        {
            try
            {
                var user = Application.Security?.User as XafAppUser;
                if (user == null) return true;
                return user.Roles.Any(r =>
                    r.Name == "OCABR_ADMIN" ||
                    r.Name == "Administrators" ||
                    r.Name == "SUPERVISOR");
            }
            catch
            {
                return true; // fail open
            }
        }

        private void RefreshCurrentView(string? newStatus = null)
        {
            if (View?.CurrentObject is not AppEntity app) return;

            // Update the in-memory entity so XAF's change tracking reflects the new state
            if (!string.IsNullOrWhiteSpace(newStatus)
                && Enum.TryParse(newStatus, true, out ApplicationStatus parsed))
            {
                app.Status         = parsed;
                app.LastActivityAt = DateTime.UtcNow;
            }

            RebuildMenuItems();

            // Reload any open Application list views so the status column updates immediately
            RefreshApplicationListViews();

            // The Application detail view uses a custom Blazor component (ApplicationDetailView.razor)
            // that holds its own _app state loaded from IApplicationRepository. Simply refreshing
            // the XAF ObjectSpace doesn't notify the component. The reliable fix is to close and
            // reopen the detail view — this forces the component to reload from DB with fresh data.
            var applicationId = app.Id;
            var caseNumber    = app.CaseNumber;

            // Close the current detail view tab
            if (Frame is NestedFrame nestedFrame)
            {
                // Nested frame — just refresh the object space
                ObjectSpace.Refresh();
                return;
            }

            // Top-level frame — close and reopen the detail view
            try
            {
                // Navigate to the list view first, then reopen the detail view
                var listViewId = Application.FindDetailViewId(typeof(AppEntity))
                    ?.Replace("_DetailView", "_ListView") ?? "Application_ListView";

                var os         = Application.CreateObjectSpace(typeof(AppEntity));
                var freshApp   = os.GetObjectByKey<AppEntity>(applicationId);

                if (freshApp != null)
                {
                    var detailView = Application.CreateDetailView(os, freshApp, true);
                    detailView.Caption = caseNumber;

                    var svp = new ShowViewParameters(detailView)
                    {
                        TargetWindow = TargetWindow.Current
                    };
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(Frame, null));
                }
                else
                {
                    // Fallback: just refresh the object space
                    ObjectSpace.Refresh();
                }
            }
            catch
            {
                // Fallback: refresh object space if navigation fails
                ObjectSpace.Refresh();
            }
        }

        private void ShowSuccess(string msg) =>
            Application.ShowViewStrategy.ShowMessage(msg, InformationType.Success, 5000, InformationPosition.Top);

        private void ShowError(string msg) =>
            Application.ShowViewStrategy.ShowMessage(msg, InformationType.Error, 5000, InformationPosition.Top);

        private void ShowWarning(string msg) =>
            Application.ShowViewStrategy.ShowMessage(msg, InformationType.Warning, 4000, InformationPosition.Top);

        /// <summary>
        /// Reloads the collection source on any open Application list view so the
        /// status column updates immediately without requiring manual navigation.
        /// </summary>
        private void RefreshApplicationListViews()
        {
            try
            {
                // The main window's view may itself be a list view, or child frames may contain one.
                // Use the RefreshController on the main window to trigger a reload.
                Application.MainWindow?.GetController<RefreshController>()?.RefreshAction.DoExecute();
            }
            catch { /* non-critical */ }
        }
    }
}