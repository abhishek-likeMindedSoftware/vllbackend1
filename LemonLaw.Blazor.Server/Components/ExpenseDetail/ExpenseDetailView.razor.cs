using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Blazor.Server.Components.ExpenseDetail;

public partial class ExpenseDetailView : ComponentBase
{
    [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;

    [Parameter] public Expense? CurrentObject { get; set; }

    // ── State ─────────────────────────────────────────────────────────────────

    private Expense? _expense;
    private bool _loading = true;
    private string? _loadError;

    private bool _showEdit;
    private bool _saving;
    private string? _saveError;
    private ExpenseFormModel? _form;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    protected override async Task OnParametersSetAsync()
    {
        if (CurrentObject == null)
        {
            _loading = false;
            _loadError = "No expense selected.";
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
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Expense>>();
            _expense = await repo.GetByIdAsync(id);

            if (_expense == null)
            {
                _loadError = "Expense not found.";
                return;
            }

            if (_expense.ApplicationId.HasValue)
            {
                var appRepo = scope.ServiceProvider.GetRequiredService<IApplicationRepository>();
                _expense.Application = await appRepo.GetWithFullDetailsAsync(_expense.ApplicationId.Value);
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
        if (_expense == null) return;
        await LoadAsync(_expense.Id);
        StateHasChanged();
    }

    // ── Edit popup ────────────────────────────────────────────────────────────

    private void OpenEdit()
    {
        if (_expense == null) return;
        _form = new ExpenseFormModel(_expense);
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
        if (_form == null || _expense == null) return;

        _saving = true;
        _saveError = null;
        try
        {
            using var scope = ServiceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Expense>>();
            _form.ApplyTo(_expense);
            repo.Update(_expense);
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

    private sealed class ExpenseFormModel
    {
        public ExpenseType ExpenseType { get; set; }
        public DateOnly? ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool ReceiptUploaded { get; set; }

        public ExpenseFormModel() { }

        public ExpenseFormModel(Expense e)
        {
            ExpenseType     = e.ExpenseType;
            ExpenseDate     = e.ExpenseDate;
            Amount          = e.Amount;
            Description     = e.Description;
            ReceiptUploaded = e.ReceiptUploaded;
        }

        public void ApplyTo(Expense e)
        {
            e.ExpenseType     = ExpenseType;
            e.ExpenseDate     = ExpenseDate;
            e.Amount          = Amount;
            e.Description     = Description;
            e.ReceiptUploaded = ReceiptUploaded;
        }
    }
}
