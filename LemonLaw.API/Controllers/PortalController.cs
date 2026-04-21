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
    LemonLawAPIDbContext context) : BaseController
{
    /// <summary>Get current case status and milestone timeline.</summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(CommonResponseDto<PortalStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus([FromQuery] Guid applicationId, [FromQuery] string? token)
    {
        // Validate consumer token per spec §2.8 / §8.1
        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenHash = HashToken(token);
            var appToken = await context.ApplicationTokens
                .FirstOrDefaultAsync(t =>
                    t.ApplicationId == applicationId &&
                    t.TokenHash == tokenHash &&
                    !t.IsRevoked &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (appToken == null)
                return Unauthorized(new CommonResponseDto<PortalStatusDto>
                {
                    Success = false,
                    Message = "Invalid or expired access link. Please use the link from your confirmation email."
                });

            // Refresh LastUsedAt per spec
            appToken.LastUsedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        var result = await applicationService.GetPortalStatusAsync(applicationId);
        return Ok(result);
    }

    /// <summary>Get consumer-visible documents for an application.</summary>
    [HttpGet("documents")]
    [ProducesResponseType(typeof(CommonResponseDto<List<DocumentListItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocuments([FromQuery] Guid applicationId, [FromQuery] string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            var tokenHash = HashToken(token);
            var appToken = await context.ApplicationTokens
                .FirstOrDefaultAsync(t =>
                    t.ApplicationId == applicationId &&
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

        var result = await applicationService.GetConsumerDocumentsAsync(applicationId);
        return Ok(result);
    }

    private static string HashToken(string token)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
