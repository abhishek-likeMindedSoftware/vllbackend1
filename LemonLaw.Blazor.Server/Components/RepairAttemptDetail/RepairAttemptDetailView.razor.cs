using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.RepairAttemptDetail;

public partial class RepairAttemptDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public RepairAttempt? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private RepairAttempt? _repair;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private RepairFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No repair attempt selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<RepairAttempt>>();
            _repair = await repo.GetByIdAsync(id);

            if (_repair == null)
            {
                _loadError = "Repair attempt not found.";
                return;
            }

            if (_repair.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _repair.Application = await appRepo.GetWithFullDetailsAsync(_repair.ApplicationId.Value);
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
        if (_repair == null) return;
        await LoadAsync(_repair.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_repair == null) return;
        _form = new RepairFormModel(_repair);
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
        if (_form == null || _repair == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<RepairAttempt>>();
            _form.ApplyTo(_repair);
            repo.Update(_repair);
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

    private sealed class RepairFormModel
    {
        public DateOnly RepairDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public string RepairFacilityName { get; set; } = string.Empty;
        public string? RepairFacilityAddr { get; set; }
        public string? RoNumber { get; set; }
        public int? MileageAtRepair { get; set; }
        public string DefectsAddressed { get; set; } = string.Empty;
        public bool RepairSuccessful { get; set; }
        public int? DaysOutOfService { get; set; }

        public RepairFormModel() { }

        public RepairFormModel(RepairAttempt r)
        {
            RepairDate          = r.RepairDate;
            RepairFacilityName  = r.RepairFacilityName;
            RepairFacilityAddr  = r.RepairFacilityAddr;
            RoNumber            = r.RoNumber;
            MileageAtRepair     = r.MileageAtRepair;
            DefectsAddressed    = r.DefectsAddressed;
            RepairSuccessful    = r.RepairSuccessful;
            DaysOutOfService    = r.DaysOutOfService;
        }

        public void ApplyTo(RepairAttempt r)
        {
            r.RepairDate         = RepairDate;
            r.RepairFacilityName = RepairFacilityName;
            r.RepairFacilityAddr = RepairFacilityAddr;
            r.RoNumber           = RoNumber;
            r.MileageAtRepair    = MileageAtRepair;
            r.DefectsAddressed   = DefectsAddressed;
            r.RepairSuccessful   = RepairSuccessful;
            r.DaysOutOfService   = DaysOutOfService;
        }
    }
}
