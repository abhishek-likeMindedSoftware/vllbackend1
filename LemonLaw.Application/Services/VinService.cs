using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LemonLaw.Application.Services;

public class VinService(
    IApplicationRepository applicationRepository,
    IMemoryCache cache,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<VinService> logger) : IVinService
{
    private const string CacheKeyPrefix = "vin_";

    public async Task<CommonResponseDto<VinLookupResponseDto>> LookupVinAsync(
        string vin, Guid? excludeApplicationId = null)
    {
        try
        {
            vin = vin.ToUpperInvariant().Trim();

            if (vin.Length != 17)
                return Fail("VIN must be exactly 17 characters.");

            // Check cache first
            var cacheKey = $"{CacheKeyPrefix}{vin}";
            VinDecodeResult? decoded;

            if (!cache.TryGetValue(cacheKey, out decoded))
            {
                decoded = await CallVinApiAsync(vin);
                if (decoded != null)
                    cache.Set(cacheKey, decoded, TimeSpan.FromHours(24));
            }

            if (decoded == null || !decoded.IsValid)
            {
                return new CommonResponseDto<VinLookupResponseDto>
                {
                    Success = true,
                    Data = new VinLookupResponseDto
                    {
                        IsValid = false,
                        ErrorMessage = "This VIN could not be decoded. Please verify the number and try again."
                    }
                };
            }

            var isDuplicate = await applicationRepository.VinHasActiveApplicationAsync(vin, excludeApplicationId);
            var duplicateCases = isDuplicate
                ? await applicationRepository.GetActiveApplicationCaseNumbersByVinAsync(vin)
                : new List<string>();

            return new CommonResponseDto<VinLookupResponseDto>
            {
                Success = true,
                Data = new VinLookupResponseDto
                {
                    IsValid = true,
                    Year = decoded.Year,
                    Make = decoded.Make,
                    Model = decoded.Model,
                    Trim = decoded.Trim,
                    IsDuplicate = isDuplicate,
                    DuplicateCaseNumbers = duplicateCases
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error looking up VIN {VIN}", vin);
            return Fail("VIN lookup failed. Please try again.");
        }
    }

    private async Task<VinDecodeResult?> CallVinApiAsync(string vin)
    {
        try
        {
            // Uses NHTSA free VIN decode API as default
            var baseUrl = configuration["VinApi:BaseUrl"]
                ?? $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";

            var client = httpClientFactory.CreateClient("VinApi");
            var response = await client.GetFromJsonAsync<NhtsaVinResponse>(baseUrl);

            if (response?.Results == null) return null;

            var yearResult = response.Results.FirstOrDefault(r => r.Variable == "Model Year");
            var makeResult = response.Results.FirstOrDefault(r => r.Variable == "Make");
            var modelResult = response.Results.FirstOrDefault(r => r.Variable == "Model");
            var trimResult = response.Results.FirstOrDefault(r => r.Variable == "Trim");
            var errorResult = response.Results.FirstOrDefault(r => r.Variable == "Error Code");

            if (errorResult?.Value == "0" || (makeResult?.Value != null && makeResult.Value != ""))
            {
                return new VinDecodeResult
                {
                    IsValid = true,
                    Year = short.TryParse(yearResult?.Value, out var y) ? y : null,
                    Make = makeResult?.Value,
                    Model = modelResult?.Value,
                    Trim = trimResult?.Value
                };
            }

            return new VinDecodeResult { IsValid = false };
        }
        catch
        {
            return null;
        }
    }

    private static CommonResponseDto<VinLookupResponseDto> Fail(string message) =>
        new() { Success = false, Message = message };

    private record VinDecodeResult
    {
        public bool IsValid { get; init; }
        public short? Year { get; init; }
        public string? Make { get; init; }
        public string? Model { get; init; }
        public string? Trim { get; init; }
    }

    private record NhtsaVinResponse
    {
        public List<NhtsaResult>? Results { get; init; }
    }

    private record NhtsaResult
    {
        public string? Variable { get; init; }
        public string? Value { get; init; }
    }
}
