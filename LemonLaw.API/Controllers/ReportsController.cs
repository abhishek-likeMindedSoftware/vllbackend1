using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Reporting endpoints for supervisor dashboard.</summary>
[Route("api/reports")]
[Authorize]
public class ReportsController(IReportingService reportingService) : BaseController
{
    /// <summary>Open cases grouped by status.</summary>
    [HttpGet("open-cases-by-status")]
    [ProducesResponseType(typeof(CommonResponseDto<List<OpenCasesByStatusDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> OpenCasesByStatus()
    {
        var result = await reportingService.GetOpenCasesByStatusAsync();
        return Ok(result);
    }

    /// <summary>Aging report — cases grouped by days since submission.</summary>
    [HttpGet("aging")]
    [ProducesResponseType(typeof(CommonResponseDto<List<AgingReportItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Aging()
    {
        var result = await reportingService.GetAgingReportAsync();
        return Ok(result);
    }

    /// <summary>Monthly case volume by application type.</summary>
    [HttpGet("volume")]
    [ProducesResponseType(typeof(CommonResponseDto<List<VolumeReportItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Volume([FromQuery] int year = 0)
    {
        if (year == 0) year = DateTime.UtcNow.Year;
        var result = await reportingService.GetVolumeReportAsync(year);
        return Ok(result);
    }

    /// <summary>Dealer response rate — % of outreach emails responded to within 15 days.</summary>
    [HttpGet("dealer-response-rate")]
    [ProducesResponseType(typeof(CommonResponseDto<DealerResponseRateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DealerResponseRate()
    {
        var result = await reportingService.GetDealerResponseRateAsync();
        return Ok(result);
    }

    /// <summary>Staff workload — open cases per assigned staff member.</summary>
    [HttpGet("staff-workload")]
    [ProducesResponseType(typeof(CommonResponseDto<List<StaffWorkloadDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> StaffWorkload()
    {
        var result = await reportingService.GetStaffWorkloadAsync();
        return Ok(result);
    }

    /// <summary>Decision summary — outcomes by type for a date range.</summary>
    [HttpGet("decision-summary")]
    [ProducesResponseType(typeof(CommonResponseDto<List<DecisionSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DecisionSummary(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var fromDate = from ?? DateTime.UtcNow.AddYears(-1);
        var toDate = to ?? DateTime.UtcNow;
        var result = await reportingService.GetDecisionSummaryAsync(fromDate, toDate);
        return Ok(result);
    }
}
