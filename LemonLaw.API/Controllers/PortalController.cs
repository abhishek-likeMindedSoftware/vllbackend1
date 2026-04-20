using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Consumer portal status and document endpoints (token-authenticated).</summary>
[Route("api/portal")]
[AllowAnonymous]
public class PortalController(IApplicationService applicationService) : BaseController
{
    /// <summary>Get current case status and milestone timeline.</summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(CommonResponseDto<PortalStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus([FromQuery] Guid applicationId)
    {
        var result = await applicationService.GetPortalStatusAsync(applicationId);
        return Ok(result);
    }

    /// <summary>Get consumer-visible documents for an application.</summary>
    [HttpGet("documents")]
    [ProducesResponseType(typeof(CommonResponseDto<List<DocumentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocuments([FromQuery] Guid applicationId)
    {
        var result = await applicationService.GetConsumerDocumentsAsync(applicationId);
        return Ok(result);
    }
}
