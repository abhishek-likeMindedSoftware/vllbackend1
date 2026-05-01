using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using LemonLaw.Application.Interfaces;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.DependencyInjection;

using XafAppUser = LemonLaw.Core.Entities.ApplicationUser;

namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Provides the "Follow-Up Actions" dropdown on the DealerOutreach detail view.
    /// Delegates directly to IDealerOutreachService — no HTTP calls.
    ///
    /// Sequence per spec §4.4 (strictly enforced):
    ///   Initial sent → FOLLOW_UP_1 available
    ///   Follow-Up 1  → FOLLOW_UP_2 available
    ///   Follow-Up 2  → FINAL_NOTICE available
    ///   Final Notice sent (or dealer responded) → dropdown hidden
    /// </summary>
    public class DealerOutreachActionsController : ViewController<DetailView>
    {
        private const string KEY_FOLLOW_UP_1  = "FollowUp1";
        private const string KEY_FOLLOW_UP_2  = "FollowUp2";
        private const string KEY_FINAL_NOTICE = "FinalNotice";

        private readonly SingleChoiceAction _followUpMenu;

        public DealerOutreachActionsController()
        {
            _followUpMenu = new SingleChoiceAction(this, "DealerFollowUpMenu", PredefinedCategory.RecordEdit)
            {
                Caption          = "Follow-Up Actions",
                ToolTip          = "Send the next follow-up or final notice to the dealer.",
                ImageName        = "Action_Send",
                PaintStyle       = ActionItemPaintStyle.CaptionAndImage,
                ShowItemsOnClick = true,
                ItemType         = SingleChoiceActionItemType.ItemIsOperation
            };
            _followUpMenu.Execute += OnFollowUpSelected;
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────

        protected override void OnActivated()
        {
            base.OnActivated();
            RebuildMenuItems();
        }

        protected override void OnViewChanging(View view)
        {
            base.OnViewChanging(view);
            RebuildMenuItems();
        }

        // ── Menu building ─────────────────────────────────────────────────────

        private void RebuildMenuItems()
        {
            _followUpMenu.Items.Clear();

            if (View?.CurrentObject is not DealerOutreach outreach)
            {
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }

            _followUpMenu.Active.SetItemValue("IsDealerOutreachView", true);

            // Hide when dealer has responded or initial outreach not yet sent
            if (outreach.Status == OutreachStatus.RESPONDED || !outreach.SentAt.HasValue)
            {
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
                return;
            }

            // Show only the next step in the sequence
            if (outreach.FollowUp1SentAt == null)
                AddItem(KEY_FOLLOW_UP_1, "Send Follow-Up 1", "Action_Send");
            else if (outreach.FollowUp2SentAt == null)
                AddItem(KEY_FOLLOW_UP_2, "Send Follow-Up 2", "Action_Send");
            else if (outreach.FinalNoticeSentAt == null)
                AddItem(KEY_FINAL_NOTICE, "Send Final Notice", "Action_Send");
            else
            {
                // All follow-ups sent — nothing left to do
                _followUpMenu.Active.SetItemValue("IsDealerOutreachView", false);
            }
        }

        private void AddItem(string key, string caption, string imageName) =>
            _followUpMenu.Items.Add(new ChoiceActionItem(caption, null) { Id = key, ImageName = imageName });

        // ── Dispatch ──────────────────────────────────────────────────────────

        private void OnFollowUpSelected(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            var outreachType = e.SelectedChoiceActionItem?.Id switch
            {
                KEY_FOLLOW_UP_1  => "FOLLOW_UP_1",
                KEY_FOLLOW_UP_2  => "FOLLOW_UP_2",
                KEY_FINAL_NOTICE => "FINAL_NOTICE",
                _                => null
            };

            if (outreachType != null)
                ExecuteFollowUp(outreachType);
        }

        // ── Send follow-up via service ────────────────────────────────────────

        private async void ExecuteFollowUp(string outreachType)
        {
            if (View?.CurrentObject is not DealerOutreach outreach) return;

            var label = outreachType switch
            {
                "FOLLOW_UP_1"  => "Follow-Up 1",
                "FOLLOW_UP_2"  => "Follow-Up 2",
                "FINAL_NOTICE" => "Final Notice",
                _              => outreachType
            };

            _followUpMenu.Enabled.SetItemValue("Busy", false);
            try
            {
                var svc    = Application.ServiceProvider!.GetRequiredService<IDealerOutreachService>();
                var staffId = (Application.Security?.User as XafAppUser)?.UserName ?? "Staff";
                var dto    = new SendFollowUpDto { OutreachType = outreachType };

                var result = await svc.SendFollowUpAsync(outreach.Id, dto, staffId);

                if (result.Success)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        $"✓ {label} sent to {outreach.DealerEmail}.",
                        InformationType.Success, 5000, InformationPosition.Top);

                    // Refresh the view so timestamps update and dropdown rebuilds
                    ObjectSpace.Refresh();
                    View?.ObjectSpace?.Refresh();
                    RebuildMenuItems();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(
                        $"{label} failed: {result.Message}",
                        InformationType.Error, 5000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage(
                    $"Error sending {label}: {ex.Message}",
                    InformationType.Error, 6000, InformationPosition.Top);
            }
            finally
            {
                _followUpMenu.Enabled.SetItemValue("Busy", true);
            }
        }
    }
}
