using LemonLaw.Application.Interfaces;
using LemonLaw.Infrastructure.Data;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LemonLaw.API.Controllers;

/// <summary>Consumer portal status and document endpoints (token-authenticated).</summary>
[Route("api/portal")]
[AllowAnonymous]
public class PortalController(
    IApplicationService applicationService,
    IDocumentService documentService,
    LemonLawAPIDbContext context) : BaseController
{
    /// <summary>
    /// Get current case status and milestone timeline.
    /// Accepts caseNumber (e.g. LL-2026-00142) — never the internal GUID.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(CommonResponseDto<PortalStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus(
        [FromQuery] string caseNumber,
        [FromQuery] string? token)
    {
        if (string.IsNullOrWhiteSpace(caseNumber))
            return BadRequest(new CommonResponseDto<PortalStatusDto>
            {
                Success = false,
                Message = "Case number is required."
            });

        // If a token is provided, validate it against the resolved application
        if (!string.IsNullOrWhiteSpace(token))
        {
            var appId = await applicationService.ResolveApplicationIdByCaseNumberAsync(caseNumber);
            if (appId == null)
                return NotFound(new CommonResponseDto<PortalStatusDto>
                {
                    Success = false,
                    Message = "No application found with that case number."
                });

            var tokenHash = HashToken(token);
            var appToken = await context.ApplicationTokens
                .FirstOrDefaultAsync(t =>
                    t.ApplicationId == appId &&
                    t.TokenHash == tokenHash &&
                    !t.IsRevoked &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (appToken == null)
                return Unauthorized(new CommonResponseDto<PortalStatusDto>
                {
                    Success = false,
                    Message = "Invalid or expired access link. Please use the link from your confirmation email."
                });

            // Refresh LastUsedAt per spec §2.8
            appToken.LastUsedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        var result = await applicationService.GetPortalStatusByCaseNumberAsync(caseNumber);
        return Ok(result);
    }

    /// <summary>Get consumer-visible documents for an application by case number.</summary>
    [HttpGet("documents")]
    [ProducesResponseType(typeof(CommonResponseDto<List<DocumentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocuments(
        [FromQuery] string caseNumber,
        [FromQuery] string? token)
    {
        if (string.IsNullOrWhiteSpace(caseNumber))
            return BadRequest(new CommonResponseDto<List<DocumentListItemDto>>
            {
                Success = false,
                Message = "Case number is required."
            });

        var appId = await applicationService.ResolveApplicationIdByCaseNumberAsync(caseNumber);
        if (appId == null)
            return NotFound(new CommonResponseDto<List<DocumentListItemDto>>
            {
                Success = false,
                Message = "No application found with that case number."
            });

        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenHash = HashToken(token);
            var appToken = await context.ApplicationTokens
                .FirstOrDefaultAsync(t =>
                    t.ApplicationId == appId &&
                    t.TokenHash == tokenHash &&
                    !t.IsRevoked &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (appToken == null)
                return Unauthorized(new CommonResponseDto<List<DocumentListItemDto>>
                {
                    Success = false,
                    Message = "Invalid or expired access link."
                });
        }

        var result = await applicationService.GetConsumerDocumentsAsync(appId.Value);
        return Ok(result);
    }

    /// <summary>
    /// Consumer uploads additional requested documents post-submission.
    /// Accepts caseNumber to identify the application.
    /// </summary>
    [HttpPost("documents")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(CommonResponseDto<DocumentUploadResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadDocument(
        [FromQuery] string caseNumber,
        [FromQuery] string? token,
        IFormFile file,
        [FromForm] string documentType)
    {
        if (string.IsNullOrWhiteSpace(caseNumber))
            return BadRequest(new CommonResponseDto<DocumentUploadResultDto>
            {
                Success = false,
                Message = "Case number is required."
            });

        var appId = await applicationService.ResolveApplicationIdByCaseNumberAsync(caseNumber);
        if (appId == null)
            return NotFound(new CommonResponseDto<DocumentUploadResultDto>
            {
                Success = false,
                Message = "No application found with that case number."
            });

        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenHash = HashToken(token);
            var appToken = await context.ApplicationTokens
                .FirstOrDefaultAsync(t =>
                    t.ApplicationId == appId &&
                    t.TokenHash == tokenHash &&
                    !t.IsRevoked &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (appToken == null)
                return Unauthorized(new CommonResponseDto<DocumentUploadResultDto>
                {
                    Success = false,
                    Message = "Invalid or expired access link."
                });
        }

        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        await using var stream = file.OpenReadStream();
        var result = await documentService.UploadDocumentAsync(
            appId.Value,
            documentType,
            file.FileName,
            file.ContentType,
            file.Length,
            stream,
            "CONSUMER");

        return Ok(result);
    }

    private static string HashToken(string token)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
