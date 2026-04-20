using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IVinService
{
    Task<CommonResponseDto<VinLookupResponseDto>> LookupVinAsync(string vin, Guid? excludeApplicationId = null);
}
