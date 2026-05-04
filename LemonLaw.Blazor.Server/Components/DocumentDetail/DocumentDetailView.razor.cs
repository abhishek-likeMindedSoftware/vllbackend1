using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.DocumentDetail;

public partial class DocumentDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public ApplicationDocument? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private ApplicationDocument? _document;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private DocFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No document selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<ApplicationDocument>>();
            _document = await repo.GetByIdAsync(id);

            if (_document == null)
            {
                _loadError = "Document not found.";
                return;
            }

            if (_document.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _document.Application = await appRepo.GetWithFullDetailsAsync(_document.ApplicationId.Value);
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
        if (_document == null) return;
        await LoadAsync(_document.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_document == null) return;
        _form = new DocFormModel(_document);
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
        if (_form == null || _document == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<ApplicationDocument>>();
            _form.ApplyTo(_document);
            repo.Update(_document);
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

    private sealed class DocFormModel
    {
        public string FileName { get; set; } = string.Empty;
        public DocumentStatus Status { get; set; }
        public DocumentType DocumentType { get; set; }
        public string? StaffNotes { get; set; }
        public bool IsVisibleConsumer { get; set; }
        public bool IsVisibleDealer { get; set; }

        public DocFormModel() { }

        public DocFormModel(ApplicationDocument d)
        {
            FileName         = d.FileName;
            Status           = d.Status;
            DocumentType     = d.DocumentType;
            StaffNotes       = d.StaffNotes;
            IsVisibleConsumer = d.IsVisible_Consumer;
            IsVisibleDealer  = d.IsVisible_Dealer;
        }

        public void ApplyTo(ApplicationDocument d)
        {
            d.Status           = Status;
            d.DocumentType     = DocumentType;
            d.StaffNotes       = StaffNotes;
            d.IsVisible_Consumer = IsVisibleConsumer;
            d.IsVisible_Dealer = IsVisibleDealer;
        }
    }
}
