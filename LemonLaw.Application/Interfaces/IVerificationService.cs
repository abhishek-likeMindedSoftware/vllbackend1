using LemonLaw.Shared.DTOs;

namespace LemonLaw.Application.Interfaces;

public interface IVerificationService
{
    Task<CommonResponseDto<bool>> SendVerificationCodeAsync(SendVerificationCodeDto dto);
    Task<CommonResponseDto<VerificationResultDto>> ConfirmVerificationCodeAsync(ConfirmVerificationCodeDto dto);
}
