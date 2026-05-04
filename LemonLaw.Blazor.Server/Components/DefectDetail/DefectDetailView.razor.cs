using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.DefectDetail;

public partial class DefectDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public Defect? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private Defect? _defect;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private DefectFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No defect selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Defect>>();
            _defect = await repo.GetByIdAsync(id);

            if (_defect == null)
            {
                _loadError = "Defect not found.";
                return;
            }

            if (_defect.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _defect.Application = await appRepo.GetWithFullDetailsAsync(_defect.ApplicationId.Value);
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
        if (_defect == null) return;
        await LoadAsync(_defect.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_defect == null) return;
        _form = new DefectFormModel(_defect);
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
        if (_form == null || _defect == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Defect>>();
            _form.ApplyTo(_defect);
            repo.Update(_defect);
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

    private sealed class DefectFormModel
    {
        public string DefectDescription { get; set; } = string.Empty;
        public DefectCategory DefectCategory { get; set; }
        public DateOnly FirstOccurrenceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public bool IsOngoing { get; set; }

        public DefectFormModel() { }

        public DefectFormModel(Defect d)
        {
            DefectDescription   = d.DefectDescription;
            DefectCategory      = d.DefectCategory;
            FirstOccurrenceDate = d.FirstOccurrenceDate;
            IsOngoing           = d.IsOngoing;
        }

        public void ApplyTo(Defect d)
        {
            d.DefectDescription   = DefectDescription;
            d.DefectCategory      = DefectCategory;
            d.FirstOccurrenceDate = FirstOccurrenceDate;
            d.IsOngoing           = IsOngoing;
        }
    }
}
