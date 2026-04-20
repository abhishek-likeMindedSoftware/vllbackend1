using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Dealer outreach and dealer portal endpoints.</summary>
[Route("api")]
public class DealerOutreachController(IDealerOutreachService dealerOutreachService) : BaseController
{
    /// <summary>Create dealer outreach and send initial email (staff).</summary>
    [HttpPost("dealer-outreach")]
    [Authorize]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOutreach([FromBody] CreateOutreachDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await dealerOutreachService.CreateOutreachAsync(dto, StaffId);
        return Ok(result);
    }

    /// <summary>Send follow-up outreach email (staff).</summary>
    [HttpPost("dealer-outreach/{id:guid}/follow-up")]
    [Authorize]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendFollowUp(Guid id, [FromBody] SendFollowUpDto dto)
    {
        var result = await dealerOutreachService.SendFollowUpAsync(id, dto, StaffId);
        return Ok(result);
    }

    /// <summary>Get case summary for dealer portal (dealer token).</summary>
    [HttpGet("dealer-portal/case-summary")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<DealerCaseSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCaseSummary([FromQuery] Guid outreachId)
    {
        var result = await dealerOutreachService.GetCaseSummaryForDealerAsync(outreachId);
        return Ok(result);
    }

    /// <summary>Submit dealer response (dealer token).</summary>
    [HttpPost("dealer-portal/response")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitResponse(
        [FromQuery] Guid outreachId,
        [FromBody] DealerResponseSubmitDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await dealerOutreachService.SubmitDealerResponseAsync(outreachId, dto, ClientIpAddress);
        return Ok(result);
    }
}
