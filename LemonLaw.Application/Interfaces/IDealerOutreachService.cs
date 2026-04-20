using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IDealerOutreachService
{
    Task<CommonResponseDto<bool>> CreateOutreachAsync(CreateOutreachDto dto, string staffId);
    Task<CommonResponseDto<bool>> SendFollowUpAsync(Guid outreachId, SendFollowUpDto dto, string staffId);
    Task<CommonResponseDto<DealerCaseSummaryDto>> GetCaseSummaryForDealerAsync(Guid outreachId);
    Task<CommonResponseDto<bool>> SubmitDealerResponseAsync(
        Guid outreachId, DealerResponseSubmitDto dto, string ipAddress);
               
}
