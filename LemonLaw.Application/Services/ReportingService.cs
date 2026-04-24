using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

public class ReportingService(
    IApplicationRepository applicationRepository,
    IGenericRepository<DealerOutreach> outreachRepository,
    IGenericRepository<DealerResponse> responseRepository,
    IGenericRepository<Decision> decisionRepository,
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

    /// <summary>
    /// Dealer response rate — % of outreach emails that received a dealer response
    /// within 15 days per spec §3.8.
    /// </summary>
    public async Task<CommonResponseDto<DealerResponseRateDto>> GetDealerResponseRateAsync()
    {
        try
        {
            var allOutreach = await outreachRepository.FindAsync(
                o => o.OutreachType == OutreachType.INITIAL && o.SentAt.HasValue);

            var outreachList = allOutreach.ToList();
            var total = outreachList.Count;

            int within15 = 0;
            foreach (var o in outreachList)
            {
                if (o.Status == OutreachStatus.RESPONDED && o.SentAt.HasValue)
                {
                    // Check if a response was submitted within 15 days of outreach
                    var deadline = o.SentAt.Value.AddDays(15);
                    var response = await responseRepository.FindOneAsync(
                        r => r.OutreachId == o.Id && r.SubmittedAt <= deadline);
                    if (response != null) within15++;
                }
            }

            var rate = total > 0 ? Math.Round((double)within15 / total * 100, 1) : 0;

            return new CommonResponseDto<DealerResponseRateDto>
            {
                Success = true,
                Data = new DealerResponseRateDto
                {
                    TotalOutreachSent = total,
                    ResponsesWithin15Days = within15,
                    ResponseRatePercent = rate
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting dealer response rate");
            return new CommonResponseDto<DealerResponseRateDto>
            {
                Success = false,
                Message = "Failed to retrieve dealer response rate."
            };
        }
    }

    /// <summary>
    /// Staff workload — open cases per assigned staff member per spec §3.8.
    /// </summary>
    public async Task<CommonResponseDto<List<StaffWorkloadDto>>> GetStaffWorkloadAsync()
    {
        try
        {
            var closedStatuses = new[] { ApplicationStatus.CLOSED, ApplicationStatus.WITHDRAWN };
            var openCases = await applicationRepository.FindAsync(
                a => !closedStatuses.Contains(a.Status) && a.AssignedToStaffId != null);

            var data = openCases
                .GroupBy(a => new { a.AssignedToStaffId, a.AssignedToName })
                .Select(g => new StaffWorkloadDto
                {
                    StaffId = g.Key.AssignedToStaffId ?? string.Empty,
                    StaffName = g.Key.AssignedToName ?? "Unknown",
                    OpenCaseCount = g.Count()
                })
                .OrderByDescending(x => x.OpenCaseCount)
                .ToList();

            return new CommonResponseDto<List<StaffWorkloadDto>> { Success = true, Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting staff workload");
            return new CommonResponseDto<List<StaffWorkloadDto>>
            {
                Success = false,
                Message = "Failed to retrieve staff workload."
            };
        }
    }

    /// <summary>
    /// Decision summary — outcomes by type for a selected date range per spec §3.8.
    /// </summary>
    public async Task<CommonResponseDto<List<DecisionSummaryDto>>> GetDecisionSummaryAsync(
        DateTime from, DateTime to)
    {
        try
        {
            var decisions = await decisionRepository.FindAsync(
                d => d.DecisionDate >= DateOnly.FromDateTime(from) &&
                     d.DecisionDate <= DateOnly.FromDateTime(to));

            var data = decisions
                .GroupBy(d => d.DecisionType)
                .Select(g => new DecisionSummaryDto
                {
                    DecisionType = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return new CommonResponseDto<List<DecisionSummaryDto>> { Success = true, Data = data };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting decision summary");
            return new CommonResponseDto<List<DecisionSummaryDto>>
            {
                Success = false,
                Message = "Failed to retrieve decision summary."
            };
        }
    }
}
