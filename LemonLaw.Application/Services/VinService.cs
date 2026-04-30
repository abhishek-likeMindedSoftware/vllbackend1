using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Shared.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LemonLaw.Application.Services;

public class VinService(
    IApplicationRepository applicationRepository,
    IMemoryCache cache,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration,
    ILogger<VinService> logger) : IVinService
{
    private const string CacheKeyPrefix = "vin_";

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<CommonResponseDto<VinLookupResponseDto>> LookupVinAsync(
        string vin, Guid? excludeApplicationId = null)
    {
        try
        {
            vin = vin.ToUpperInvariant().Trim();

            if (vin.Length != 17)
                return Fail("VIN must be exactly 17 characters.");

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
            return new CommonResponseDto<VinLookupResponseDto>
            {
                Success = false,
                Message = "VIN lookup failed. Please try again.",
                ExceptionMessage = ex.Message
            };
        }
    }

    private async Task<VinDecodeResult?> CallVinApiAsync(string vin)
    {
        try
        {
            var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";

            var client = httpClientFactory.CreateClient("VinApi");

            // Must explicitly set Accept: application/json — NHTSA returns XML otherwise
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var httpResponse = await client.SendAsync(request);
            if (!httpResponse.IsSuccessStatusCode) return null;

            var json = await httpResponse.Content.ReadAsStringAsync();
            logger.LogDebug("NHTSA response for {VIN}: {Json}", vin, json[..Math.Min(500, json.Length)]);

            var response = JsonSerializer.Deserialize<NhtsaVinResponse>(json, _jsonOptions);
            if (response?.Results == null) return null;

            var make  = response.Results.FirstOrDefault(r => r.Variable == "Make")?.Value;
            var year  = response.Results.FirstOrDefault(r => r.Variable == "Model Year")?.Value;
            var model = response.Results.FirstOrDefault(r => r.Variable == "Model")?.Value;
            var trim  = response.Results.FirstOrDefault(r => r.Variable == "Trim")?.Value;

            // Valid if Make is present — NHTSA may return non-zero error codes for partial data
            if (string.IsNullOrWhiteSpace(make))
                return new VinDecodeResult { IsValid = false };

            return new VinDecodeResult
            {
                IsValid = true,
                Year  = short.TryParse(year, out var y) ? y : null,
                Make  = make,
                Model = string.IsNullOrWhiteSpace(model) ? null : model,
                Trim  = string.IsNullOrWhiteSpace(trim)  ? null : trim
            };
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "NHTSA VIN API call failed for VIN {VIN}", vin);
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
        public string? Value    { get; init; }
    }
}
