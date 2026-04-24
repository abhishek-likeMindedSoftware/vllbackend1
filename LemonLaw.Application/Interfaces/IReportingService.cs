using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IReportingService
{
    Task<CommonResponseDto<List<OpenCasesByStatusDto>>> GetOpenCasesByStatusAsync();
    Task<CommonResponseDto<List<AgingReportItemDto>>> GetAgingReportAsync();
    Task<CommonResponseDto<List<VolumeReportItemDto>>> GetVolumeReportAsync(int year);
    Task<CommonResponseDto<DealerResponseRateDto>> GetDealerResponseRateAsync();
    Task<CommonResponseDto<List<StaffWorkloadDto>>> GetStaffWorkloadAsync();
    Task<CommonResponseDto<List<DecisionSummaryDto>>> GetDecisionSummaryAsync(DateTime from, DateTime to);
}
