using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>VIN lookup and duplicate check endpoints.</summary>
[Route("api/vin")]
public class VinController(IVinService vinService) : BaseController
{
    /// <summary>Decode a VIN and check for duplicate active applications.</summary>
    [HttpGet("lookup/{vin}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<VinLookupResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(string vin, [FromQuery] Guid? excludeApplicationId = null)
    {
        var result = await vinService.LookupVinAsync(vin, excludeApplicationId);
        return Ok(result);
    }

    /// <summary>Staff-side duplicate VIN check.</summary>
    [HttpGet("duplicate-check/{vin}")]
    [Authorize]
    [ProducesResponseType(typeof(CommonResponseDto<VinLookupResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DuplicateCheck(string vin)
    {
        var result = await vinService.LookupVinAsync(vin);
        return Ok(result);
    }
}
