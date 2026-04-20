using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>
/// Dealer response portal — token-authenticated endpoints for dealers to
/// review case summaries and submit formal responses.
/// </summary>
[Route("api/dealer-portal")]
[AllowAnonymous] // Dealer token validated in service layer
public class DealerPortalController(IDealerOutreachService dealerOutreachService) : BaseController
{
    /// <summary>
    /// Returns the consumer-approved case summary for the dealer to review.
    /// Requires a valid dealer access token passed as a query parameter.
    /// </summary>
    [HttpGet("case-summary")]
    [ProducesResponseType(typeof(CommonResponseDto<DealerCaseSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCaseSummary([FromQuery] Guid outreachId)
    {
        var result = await dealerOutreachService.GetCaseSummaryForDealerAsync(outreachId);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Submits the dealer's formal response to the consumer's Lemon Law application.
    /// Triggers a status change to DEALER_RESPONDED and notifies OCABR staff.
    /// </summary>
    [HttpPost("response")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitResponse(
        [FromQuery] Guid outreachId,
        [FromBody] DealerResponseSubmitDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var result = await dealerOutreachService.SubmitDealerResponseAsync(
            outreachId, dto, ClientIpAddress);

        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
