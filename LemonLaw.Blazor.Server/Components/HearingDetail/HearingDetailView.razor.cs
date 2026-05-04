using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.HearingDetail;

public partial class HearingDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public Hearing? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private Hearing? _hearing;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private HearingFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No hearing selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Hearing>>();
            _hearing = await repo.GetByIdAsync(id);

            if (_hearing == null)
            {
                _loadError = "Hearing not found.";
                return;
            }

            if (_hearing.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _hearing.Application = await appRepo.GetWithFullDetailsAsync(_hearing.ApplicationId.Value);
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
        if (_hearing == null) return;
        await LoadAsync(_hearing.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_hearing == null) return;
        _form = new HearingFormModel(_hearing);
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
        if (_form == null || _hearing == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Hearing>>();
            _form.ApplyTo(_hearing);
            repo.Update(_hearing);
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

    private sealed class HearingFormModel
    {
        public DateTime HearingDate { get; set; } = DateTime.Now;
        public HearingFormat HearingFormat { get; set; }
        public string? HearingLocation { get; set; }
        public string? ArbitratorName { get; set; }
        public HearingOutcome Outcome { get; set; } = HearingOutcome.PENDING;
        public string? OutcomeNotes { get; set; }
        public DateTime? ContinuedTo { get; set; }
        public bool ConsumerNoticesSent { get; set; }
        public bool DealerNoticesSent { get; set; }

        public HearingFormModel() { }

        public HearingFormModel(Hearing h)
        {
            HearingDate        = h.HearingDate.ToLocalTime();
            HearingFormat      = h.HearingFormat;
            HearingLocation    = h.HearingLocation;
            ArbitratorName     = h.ArbitratorName;
            Outcome            = h.Outcome;
            OutcomeNotes       = h.OutcomeNotes;
            ContinuedTo        = h.ContinuedTo?.ToLocalTime();
            ConsumerNoticesSent = h.ConsumerNoticesSent;
            DealerNoticesSent  = h.DealerNoticesSent;
        }

        public void ApplyTo(Hearing h)
        {
            h.HearingDate        = DateTime.SpecifyKind(HearingDate, DateTimeKind.Local).ToUniversalTime();
            h.HearingFormat      = HearingFormat;
            h.HearingLocation    = HearingLocation;
            h.ArbitratorName     = ArbitratorName;
            h.Outcome            = Outcome;
            h.OutcomeNotes       = OutcomeNotes;
            h.ContinuedTo        = ContinuedTo.HasValue ? DateTime.SpecifyKind(ContinuedTo.Value, DateTimeKind.Local).ToUniversalTime() : null;
            h.ConsumerNoticesSent = ConsumerNoticesSent;
            h.DealerNoticesSent  = DealerNoticesSent;
        }
    }
}
