using LemonLaw.Core.Entities;
using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Repositories;

public interface IApplicationRepository : IGenericRepository<VllApplication>
{
    Task<VllApplication?> GetWithFullDetailsAsync(Guid applicationId);
    Task<(IEnumerable<VllApplication> Items, int Total)> GetPagedAsync(CaseListFilterDto filter);
    Task<string> GenerateCaseNumberAsync();
    Task<bool> VinHasActiveApplicationAsync(string vin, Guid? excludeApplicationId = null);
    Task<List<string>> GetActiveApplicationCaseNumbersByVinAsync(string vin);
    Task<VllApplication?> GetByCaseNumberAsync(string caseNumber);
}
