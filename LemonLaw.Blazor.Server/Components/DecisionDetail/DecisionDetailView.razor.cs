using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.DecisionDetail;

public partial class DecisionDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public Decision? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private Decision? _decision;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private DecisionFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No decision selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Decision>>();

            _decision = await repo.GetByIdAsync(id);

            if (_decision == null)
            {
                _loadError = "Decision not found.";
                return;
            }

            // Eagerly load Application → Vehicle via the application repository
            // (GetWithFullDetailsAsync already includes Vehicle with dealer fields)
            if (_decision.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                var app = await appRepo.GetWithFullDetailsAsync(_decision.ApplicationId.Value);
                _decision.Application = app;
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
        if (_decision == null) return;
        await LoadAsync(_decision.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_decision == null) return;
        _form = new DecisionFormModel(_decision);
        _saveError = null;
        _showEdit = true;
    }

    private void CloseEdit()
    {
        _showEdit = false;
        _form = null;
    }

    private async Task SaveAsync()
    {
        if (_form == null || _decision == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Decision>>();
            _form.ApplyTo(_decision);
            repo.Update(_decision);
            await repo.SaveChangesAsync();
            _showEdit = false;
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            _saveError = ex.Message;
        }
        finally
        {
            _saving = false;
        }
    }

    // ── Form model ────────────────────────────────────────────────────────────

    private sealed class DecisionFormModel
    {
        public DecisionType DecisionType { get; set; }
        public DateOnly DecisionDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public decimal? RefundAmount { get; set; }
        public DateOnly? ComplianceDeadline { get; set; }
        public bool ConsumerNotified { get; set; }
        public bool DealerNotified { get; set; }

        public DecisionFormModel() { }

        public DecisionFormModel(Decision d)
        {
            DecisionType      = d.DecisionType;
            DecisionDate      = d.DecisionDate;
            RefundAmount      = d.RefundAmount;
            ComplianceDeadline = d.ComplianceDeadline;
            ConsumerNotified  = d.ConsumerNotified;
            DealerNotified    = d.DealerNotified;
        }

        public void ApplyTo(Decision d)
        {
            d.DecisionType      = DecisionType;
            d.DecisionDate      = DecisionDate;
            d.RefundAmount      = RefundAmount;
            d.ComplianceDeadline = ComplianceDeadline;
            d.ConsumerNotified  = ConsumerNotified;
            d.DealerNotified    = DealerNotified;
        }
    }
}
