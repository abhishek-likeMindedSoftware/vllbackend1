using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.CorrespondenceDetail;

public partial class CorrespondenceDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public Correspondence? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private Correspondence? _correspondence;
    private bool _loading = true;
    private string? _loadError;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No correspondence selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Correspondence>>();
            _correspondence = await repo.GetByIdAsync(id);

            if (_correspondence == null)
            {
                _loadError = "Correspondence not found.";
                return;
            }

            if (_correspondence.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _correspondence.Application = await appRepo.GetWithFullDetailsAsync(_correspondence.ApplicationId.Value);
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
}
