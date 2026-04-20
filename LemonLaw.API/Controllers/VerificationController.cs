using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Email verification endpoints.</summary>
[Route("api/verification")]
[AllowAnonymous]
public class VerificationController(IVerificationService verificationService) : BaseController
{
    /// <summary>Send a one-time verification code to the provided email.</summary>
    [HttpPost("send-code")]
    [ProducesResponseType(typeof(CommonResponseDto<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendCode([FromBody] SendVerificationCodeDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await verificationService.SendVerificationCodeAsync(dto);
        return Ok(result);
    }

    /// <summary>Confirm the one-time code and receive a pre-submission token.</summary>
    [HttpPost("confirm-code")]
    [ProducesResponseType(typeof(CommonResponseDto<VerificationResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmVerificationCodeDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var result = await verificationService.ConfirmVerificationCodeAsync(dto);
        return Ok(result);
    }
}
