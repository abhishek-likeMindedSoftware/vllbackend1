using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.CaseNoteDetail;

public partial class CaseNoteDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    [Parameter] public CaseNote? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private CaseNote? _note;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private NoteFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No note selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<CaseNote>>();
            _note = await repo.GetByIdAsync(id);

            if (_note == null)
            {
                _loadError = "Note not found.";
                return;
            }

            if (_note.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _note.Application = await appRepo.GetWithFullDetailsAsync(_note.ApplicationId.Value);
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
        if (_note == null) return;
        await LoadAsync(_note.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_note == null) return;
        _form = new NoteFormModel(_note);
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
        if (_form == null || _note == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<CaseNote>>();
            _form.ApplyTo(_note);
            repo.Update(_note);
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

    private sealed class NoteFormModel
    {
        public string NoteText { get; set; } = string.Empty;
        public bool IsPinned { get; set; }

        public NoteFormModel() { }

        public NoteFormModel(CaseNote n)
        {
            NoteText = n.NoteText;
            IsPinned = n.IsPinned;
        }

        public void ApplyTo(CaseNote n)
        {
            n.NoteText = NoteText;
            n.IsPinned = IsPinned;
        }
    }
}
