using LemonLaw.Shared.DTOs;

using AppEntity = LemonLaw.Core.Entities.Application;

namespace LemonLaw.Application.Repositories;

public interface IApplicationRepository : IGenericRepository<AppEntity>
{
    Task<AppEntity?> GetWithFullDetailsAsync(Guid applicationId);
    Task<(IEnumerable<AppEntity> Items, int Total)> GetPagedAsync(CaseListFilterDto filter);
    Task<string> GenerateCaseNumberAsync();
    Task<bool> VinHasActiveApplicationAsync(string vin, Guid? excludeApplicationId = null);
    Task<List<string>> GetActiveApplicationCaseNumbersByVinAsync(string vin);
}
