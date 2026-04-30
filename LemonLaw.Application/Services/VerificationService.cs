using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

public class VerificationService(
    IMemoryCache cache,
    IEmailService emailService,
    ILogger<VerificationService> logger) : IVerificationService
{
    private const string CodePrefix    = "verify_";
    private const string AttemptPrefix = "verify_attempts_";

    public async Task<CommonResponseDto<bool>> SendVerificationCodeAsync(SendVerificationCodeDto dto)
    {
        try
        {
            var attemptsKey = $"{AttemptPrefix}{dto.EmailAddress}";
            var attempts = cache.GetOrCreate(attemptsKey, e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return 0;
            });

            if (attempts >= 3)
                return new CommonResponseDto<bool>
                {
                    Success = false,
                    Message = "Maximum resend attempts reached. Please wait before trying again."
                };

            var code = GenerateCode();
            var codeKey = $"{CodePrefix}{dto.EmailAddress}";

            cache.Set(codeKey, code, TimeSpan.FromMinutes(15));
            cache.Set(attemptsKey, attempts + 1, TimeSpan.FromHours(1));

            await emailService.SendAsync(
                dto.EmailAddress, dto.EmailAddress,
                "Your Lemon Law Application Verification Code",
                $"<p>Your verification code is: <strong>{code}</strong></p><p>This code expires in 15 minutes.</p>",
                $"Your verification code is: {code}. This code expires in 15 minutes.");

            return new CommonResponseDto<bool>
            {
                Success = true,
                Data = true,
                Message = "Verification code sent."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending verification code to {Email}", dto.EmailAddress);
            return new CommonResponseDto<bool> { Success = false, Message = "Failed to send code." };
        }
    }

    public async Task<CommonResponseDto<VerificationResultDto>> ConfirmVerificationCodeAsync(
        ConfirmVerificationCodeDto dto)
    {
        try
        {
            var codeKey = $"{CodePrefix}{dto.EmailAddress}";

            if (!cache.TryGetValue(codeKey, out string? storedCode) || storedCode != dto.Code)
            {
                return new CommonResponseDto<VerificationResultDto>
                {
                    Success = false,
                    Message = "Invalid or expired verification code.",
                    Data = new VerificationResultDto { Verified = false }
                };
            }

            var verifiedKey = $"verified_{dto.EmailAddress}";
            cache.Set(verifiedKey, true, TimeSpan.FromMinutes(30));
            cache.Remove(codeKey);

            var preSubToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            return new CommonResponseDto<VerificationResultDto>
            {
                Success = true,
                Data = new VerificationResultDto
                {
                    Verified = true,
                    PreSubmissionToken = preSubToken
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error confirming verification code for {Email}", dto.EmailAddress);
            return new CommonResponseDto<VerificationResultDto>
            {
                Success = false,
                Message = "Verification failed."
            };
        }
    }

    private static string GenerateCode()
    {
        return System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 999999).ToString();
    }
}
