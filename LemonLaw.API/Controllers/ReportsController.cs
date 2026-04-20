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
}
