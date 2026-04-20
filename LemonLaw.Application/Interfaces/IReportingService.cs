using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IReportingService
{
    Task<CommonResponseDto<List<OpenCasesByStatusDto>>> GetOpenCasesByStatusAsync();
    Task<CommonResponseDto<List<AgingReportItemDto>>> GetAgingReportAsync();
    Task<CommonResponseDto<List<VolumeReportItemDto>>> GetVolumeReportAsync(int year);
}
