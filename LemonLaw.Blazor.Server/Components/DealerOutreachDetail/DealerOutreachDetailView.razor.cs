using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.DealerOutreachDetail;

public partial class DealerOutreachDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public DealerOutreach? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private DealerOutreach? _outreach;
    private bool _loading = true;
    private string? _loadError;

    private bool _showFollowUpMenu;
    private bool _sendingFollowUp;
    private string? _followUpSuccess;
    private string? _followUpError;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No dealer outreach selected.";
            return;
        }

        await LoadAsync(CurrentObject.Id);
    }

    // ── Data loading ──────────────────────────────────────────────────────────

    private async Task LoadAsync(Guid id)
    {
        _loading = true;
        _loadError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<DealerOutreach>>();
            _outreach = await repo.GetByIdAsync(id);

            if (_outreach == null)
            {
                _loadError = "Dealer outreach not found.";
                return;
            }

            if (_outreach.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _outreach.Application = await appRepo.GetWithFullDetailsAsync(_outreach.ApplicationId.Value);
            }
        }
        catch (Exception ex)
        {
            _loadError = ex.Message;
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task RefreshAsync()
    {
        if (_outreach == null) return;
        await LoadAsync(_outreach.Id);
        StateHasChanged();
    }

    // ── Follow-Up dropdown ────────────────────────────────────────────────────

    private void ToggleFollowUpMenu()
    {
        _showFollowUpMenu = !_showFollowUpMenu;
        _followUpSuccess  = null;
        _followUpError    = null;
    }

    private async Task SendFollowUpAsync(string outreachType)
    {
        if (_outreach == null) return;

        _showFollowUpMenu = false;
        _sendingFollowUp  = true;
        _followUpSuccess  = null;
        _followUpError    = null;

        try
        {
            using var scope = ServiceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IDealerOutreachService>();

            var result = await svc.SendFollowUpAsync(
                _outreach.Id,
                new SendFollowUpDto { OutreachType = outreachType },
                "staff");

            if (result.Success)
            {
                var label = outreachType switch
                {
                    "DEALER_FOLLOW_UP_1"  => "Follow-Up 1",
                    "DEALER_FOLLOW_UP_2"  => "Follow-Up 2",
                    "DEALER_FINAL_NOTICE" => "Final Notice",
                    _                     => outreachType
                };
                _followUpSuccess = $"{label} sent successfully.";
                await RefreshAsync();
            }
            else
            {
                _followUpError = result.Message ?? "Failed to send follow-up.";
            }
        }
        catch (Exception ex)
        {
            _followUpError = ex.Message;
        }
        finally
        {
            _sendingFollowUp = false;
        }
    }
}
