using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

public class ReportingService(
    IApplicationRepository applicationRepository,
    ILogger<ReportingService> logger) : IReportingService
{
    public async Task<CommonResponseDto<List<OpenCasesByStatusDto>>> GetOpenCasesByStatusAsync()
    {
        try
        {
            var closedStatuses = new[] { ApplicationStatus.CLOSED, ApplicationStatus.WITHDRAWN };
            var all = await applicationRepository.GetAllAsync();

            var data = all
                .Where(a => !closedStatuses.Contains(a.Status))
                .GroupBy(a => a.Status)
                .Select(g => new OpenCasesByStatusDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            return new CommonResponseDto<List<OpenCasesByStatusDto>> { Success = true, Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting open cases by status");
            return new CommonResponseDto<List<OpenCasesByStatusDto>>
            {
                Success = false,
                Message = "Failed to retrieve report."
            };
        }
    }

    public async Task<CommonResponseDto<List<AgingReportItemDto>>> GetAgingReportAsync()
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var closedStatuses = new[] { ApplicationStatus.CLOSED, ApplicationStatus.WITHDRAWN };
            var applications = await applicationRepository.FindAsync(
                a => !closedStatuses.Contains(a.Status));

            var buckets = new Dictionary<string, int>
            {
                ["0-15 days"] = 0,
                ["16-30 days"] = 0,
                ["31-60 days"] = 0,
                ["60+ days"] = 0
            };

            foreach (var app in applications)
            {
                var days = (today - app.SubmittedAt.Date).Days;
                if (days <= 15) buckets["0-15 days"]++;
                else if (days <= 30) buckets["16-30 days"]++;
                else if (days <= 60) buckets["31-60 days"]++;
                else buckets["60+ days"]++;
            }

            var data = buckets.Select(b => new AgingReportItemDto
            {
                Bucket = b.Key,
                Count = b.Value
            }).ToList();

            return new CommonResponseDto<List<AgingReportItemDto>> { Success = true, Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting aging report");
            return new CommonResponseDto<List<AgingReportItemDto>>
            {
                Success = false,
                Message = "Failed to retrieve aging report."
            };
        }
    }

    public async Task<CommonResponseDto<List<VolumeReportItemDto>>> GetVolumeReportAsync(int year)
    {
        try
        {
            var applications = await applicationRepository.FindAsync(
                a => a.SubmittedAt.Year == year);

            var data = applications
                .GroupBy(a => new { a.SubmittedAt.Year, a.SubmittedAt.Month, a.ApplicationType })
                .Select(g => new VolumeReportItemDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    ApplicationType = g.Key.ApplicationType.ToString(),
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            return new CommonResponseDto<List<VolumeReportItemDto>> { Success = true, Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting volume report for year {Year}", year);
            return new CommonResponseDto<List<VolumeReportItemDto>>
            {
                Success = false,
                Message = "Failed to retrieve volume report."
            };
        }
    }
}
