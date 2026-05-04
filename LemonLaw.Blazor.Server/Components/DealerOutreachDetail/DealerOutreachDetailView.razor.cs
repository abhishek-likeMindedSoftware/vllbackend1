using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.DealerOutreachDetail;

public partial class DealerOutreachDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;
    [Inject] private IXafApplicationProvider XafAppProvider { get; set; } = default!;

    [Parameter] public DealerOutreach? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private DealerOutreach? _outreach;
    private bool _loading = true;
    private string? _loadError;

    private bool _sendingFollowUp;

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

    // ── Send follow-up ────────────────────────────────────────────────────────

    private async Task SendFollowUpAsync(string outreachType)
    {
        if (_outreach == null) return;

        _sendingFollowUp = true;

        try
        {
            using var scope = ServiceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<IDealerOutreachService>();

            var result = await svc.SendFollowUpAsync(
                _outreach.Id,
                new SendFollowUpDto { OutreachType = outreachType },
                "staff");

            var label = outreachType switch
            {
                "FOLLOW_UP_1"  => "Follow-Up 1",
                "FOLLOW_UP_2"  => "Follow-Up 2",
                "FINAL_NOTICE" => "Final Notice",
                _              => outreachType
            };

            if (result.Success)
            {
                ShowXafMessage($"✓ {label} sent successfully.", InformationType.Success);
                // Reload from DB so timestamps and button state update immediately
                await RefreshAsync();
            }
            else
            {
                ShowXafMessage($"{label} failed: {result.Message ?? "Unknown error."}", InformationType.Error);
            }
        }
        catch (Exception ex)
        {
            ShowXafMessage($"Error: {ex.Message}", InformationType.Error);
        }
        finally
        {
            _sendingFollowUp = false;
        }
    }

    // ── XAF toast helper ──────────────────────────────────────────────────────

    private void ShowXafMessage(string message, InformationType type)
    {
        try
        {
            var xafApp = XafAppProvider.GetApplication();
            xafApp.ShowViewStrategy.ShowMessage(message, type, 5000, InformationPosition.Top);
        }
        catch { /* non-critical — swallow if XAF context unavailable */ }
    }
}
