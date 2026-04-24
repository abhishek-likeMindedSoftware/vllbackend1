using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Dealer outreach and dealer portal endpoints.</summary>
[Route("api")]
public class DealerOutreachController(
    IDealerOutreachService dealerOutreachService,
    IDocumentService documentService) : BaseController
{
    /// <summary>Create dealer outreach and send initial email (staff).</summary>
    [HttpPost("dealer-outreach")]
    [AllowAnonymous] // TODO: restore [Authorize] once Azure AD is configured
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOutreach([FromBody] CreateOutreachDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await dealerOutreachService.CreateOutreachAsync(dto, StaffId);
        return Ok(result);
    }

    /// <summary>Send follow-up outreach email (staff).</summary>
    [HttpPost("dealer-outreach/{id:guid}/follow-up")]
    [AllowAnonymous] // TODO: restore [Authorize] once Azure AD is configured
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
    public async Task<IActionResult> GetCaseSummary(
        [FromQuery] Guid outreachId,
        [FromQuery] string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            var valid = await dealerOutreachService.ValidateDealerTokenAsync(outreachId, token);
            if (!valid)
                return Unauthorized(new CommonResponseDto<DealerCaseSummaryDto>
                {
                    Success = false,
                    Message = "Invalid or expired dealer access link."
                });
        }

        var result = await dealerOutreachService.GetCaseSummaryForDealerAsync(outreachId);
        return Ok(result);
    }

    /// <summary>Get documents visible to dealer (dealer token).</summary>
    [HttpGet("dealer-portal/documents")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<List<DocumentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDealerDocuments(
        [FromQuery] Guid outreachId,
        [FromQuery] string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            var valid = await dealerOutreachService.ValidateDealerTokenAsync(outreachId, token);
            if (!valid)
                return Unauthorized(new CommonResponseDto<List<DocumentListItemDto>>
                {
                    Success = false,
                    Message = "Invalid or expired dealer access link."
                });
        }

        var result = await dealerOutreachService.GetDealerDocumentsAsync(outreachId);
        return Ok(result);
    }

    /// <summary>Submit dealer response (dealer token).</summary>
    [HttpPost("dealer-portal/response")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitResponse(
        [FromQuery] Guid outreachId,
        [FromQuery] string? token,
        [FromBody] DealerResponseSubmitDto dto)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            var valid = await dealerOutreachService.ValidateDealerTokenAsync(outreachId, token);
            if (!valid)
                return Unauthorized(new CommonResponseDto<bool>
                {
                    Success = false,
                    Message = "Invalid or expired dealer access link."
                });
        }

        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await dealerOutreachService.SubmitDealerResponseAsync(outreachId, dto, ClientIpAddress);
        return Ok(result);
    }

    /// <summary>Dealer uploads supporting document (dealer token).</summary>
    [HttpPost("dealer-portal/response/documents")]
    [AllowAnonymous]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CommonResponseDto<DocumentUploadResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadDealerDocument(
        [FromQuery] Guid outreachId,
        [FromQuery] string? token,
        IFormFile file,
        [FromForm] string documentType)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            var valid = await dealerOutreachService.ValidateDealerTokenAsync(outreachId, token);
            if (!valid)
                return Unauthorized(new CommonResponseDto<DocumentUploadResultDto>
                {
                    Success = false,
                    Message = "Invalid or expired dealer access link."
                });
        }

        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        // Get applicationId from outreach
        var summary = await dealerOutreachService.GetCaseSummaryForDealerAsync(outreachId);
        if (!summary.Success)
            return BadRequest("Outreach record not found.");

        // We need the applicationId — get it from the case summary context
        // The outreach service already validated the outreachId, so we can trust it
        // We'll pass the outreachId as the applicationId placeholder and let the service resolve it
        // Actually we need to get the applicationId from the outreach record
        // For now, use the outreachId as a lookup key via the dealer documents endpoint
        // The documentService needs the applicationId — we'll get it from the outreach
        var caseSummary = summary.Data;
        if (caseSummary == null) return BadRequest("Case not found.");

        // We need to resolve applicationId from outreachId — add a helper
        // For now, upload using the outreachId as a proxy (the service will resolve it)
        await using var stream = file.OpenReadStream();
        var result = await documentService.UploadDocumentForOutreachAsync(
            outreachId, documentType, file.FileName, file.ContentType, file.Length, stream);

        return Ok(result);
    }
}
