using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Infrastructure.Data;
using LemonLaw.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Infrastructure.Repositories;

public class ApplicationRepository(LemonLawAPIDbContext context)
    : GenericRepository<AppEntity>(context), IApplicationRepository
{
    public async Task<AppEntity?> GetWithFullDetailsAsync(Guid applicationId)
    {
        return await _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Vehicle)
            .Include(a => a.Defects.OrderBy(d => d.SortOrder))
            .Include(a => a.RepairAttempts.OrderBy(r => r.SortOrder))
            .Include(a => a.Expenses)
            .Include(a => a.Documents)
            .Include(a => a.Token)
            .Include(a => a.Notes.OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.CreatedAt))
            .Include(a => a.Correspondences.OrderByDescending(c => c.SentAt))
            .Include(a => a.DealerOutreaches)
            .Include(a => a.Hearings)
            .Include(a => a.Decision)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
    }

    public async Task<(IEnumerable<AppEntity> Items, int Total)> GetPagedAsync(CaseListFilterDto filter)
    {
        var query = _context.Applications
            .Include(a => a.Applicant)
            .Include(a => a.Vehicle)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.ApplicationType))
            query = query.Where(a => a.ApplicationType.ToString() == filter.ApplicationType);

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(a => a.Status.ToString() == filter.Status);

        if (!string.IsNullOrWhiteSpace(filter.AssignedToStaffId))
            query = query.Where(a => a.AssignedToStaffId == filter.AssignedToStaffId);

        if (!string.IsNullOrWhiteSpace(filter.VIN))
            query = query.Where(a => a.Vehicle != null && a.Vehicle.VIN == filter.VIN);

        if (filter.SubmittedFrom.HasValue)
            query = query.Where(a => a.SubmittedAt >= filter.SubmittedFrom.Value);

        if (filter.SubmittedTo.HasValue)
            query = query.Where(a => a.SubmittedAt <= filter.SubmittedTo.Value);

        var total = await query.CountAsync();

        query = filter.SortBy switch
        {
            "agingDays" => filter.SortDescending
                ? query.OrderByDescending(a => a.SubmittedAt)
                : query.OrderBy(a => a.SubmittedAt),
            "lastActivityAt" => filter.SortDescending
                ? query.OrderByDescending(a => a.LastActivityAt)
                : query.OrderBy(a => a.LastActivityAt),
            _ => query.OrderByDescending(a => a.SubmittedAt)
        };

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<string> GenerateCaseNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var count = await _context.Applications
            .CountAsync(a => a.SubmittedAt.Year == year);
        return $"LL-{year}-{(count + 1):D5}";
    }

    public async Task<bool> VinHasActiveApplicationAsync(string vin, Guid? excludeApplicationId = null)
    {
        var activeStatuses = new[] { ApplicationStatus.WITHDRAWN, ApplicationStatus.CLOSED };
        var query = _context.Applications
            .Include(a => a.Vehicle)
            .Where(a => a.Vehicle != null
                        && a.Vehicle.VIN == vin
                        && !activeStatuses.Contains(a.Status));

        if (excludeApplicationId.HasValue)
            query = query.Where(a => a.Id != excludeApplicationId.Value);

        return await query.AnyAsync();
    }

    public async Task<List<string>> GetActiveApplicationCaseNumbersByVinAsync(string vin)
    {
        var activeStatuses = new[] { ApplicationStatus.WITHDRAWN, ApplicationStatus.CLOSED };
        return await _context.Applications
            .Include(a => a.Vehicle)
            .Where(a => a.Vehicle != null
                        && a.Vehicle.VIN == vin
                        && !activeStatuses.Contains(a.Status))
            .Select(a => a.CaseNumber)
            .ToListAsync();
    }
}
