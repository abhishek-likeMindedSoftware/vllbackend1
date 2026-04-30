using LemonLaw.Application.Interfaces;
using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace LemonLaw.API.Controllers;

/// <summary>VIN lookup and duplicate check endpoints.</summary>
[Route("api/vin")]
public class VinController(IVinService vinService, IHttpClientFactory httpClientFactory) : BaseController
{
    /// <summary>Decode a VIN and check for duplicate active applications.</summary>
    [HttpGet("lookup/{vin}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CommonResponseDto<VinLookupResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Lookup(string vin, [FromQuery] Guid? excludeApplicationId = null)
    {
        vin = vin.Trim();
        var result = await vinService.LookupVinAsync(vin, excludeApplicationId);
        return Ok(result);
    }

    /// <summary>Staff-side duplicate VIN check.</summary>
    [HttpGet("duplicate-check/{vin}")]
    [Authorize]
    [ProducesResponseType(typeof(CommonResponseDto<VinLookupResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DuplicateCheck(string vin)
    {
        vin = vin.Trim();
        var result = await vinService.LookupVinAsync(vin);
        return Ok(result);
    }

    /// <summary>
    /// Diagnostic endpoint — calls NHTSA directly and returns the raw response.
    /// Use this to verify connectivity and parsing. Remove before production.
    /// </summary>
    [HttpGet("debug/{vin}")]
    [AllowAnonymous]
    public async Task<IActionResult> Debug(string vin)
    {
        vin = vin.Trim();
        var diagnostics = new Dictionary<string, object?>();
        diagnostics["vin"] = vin;
        diagnostics["vinLength"] = vin.Length;

        try
        {
            var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";
            diagnostics["url"] = url;

            var client = httpClientFactory.CreateClient("VinApi");
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await client.SendAsync(request);
            diagnostics["httpStatus"] = (int)response.StatusCode;
            diagnostics["contentType"] = response.Content.Headers.ContentType?.ToString();

            var body = await response.Content.ReadAsStringAsync();
            diagnostics["responseLength"] = body.Length;
            diagnostics["responsePreview"] = body[..Math.Min(1000, body.Length)];

            // Try to parse
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var parsed = System.Text.Json.JsonSerializer.Deserialize<NhtsaDebugResponse>(body, options);

            if (parsed?.Results != null)
            {
                var make  = parsed.Results.FirstOrDefault(r => r.Variable == "Make")?.Value;
                var year  = parsed.Results.FirstOrDefault(r => r.Variable == "Model Year")?.Value;
                var model = parsed.Results.FirstOrDefault(r => r.Variable == "Model")?.Value;
                var error = parsed.Results.FirstOrDefault(r => r.Variable == "Error Code")?.Value;

                diagnostics["parsed_make"]       = make;
                diagnostics["parsed_year"]       = year;
                diagnostics["parsed_model"]      = model;
                diagnostics["parsed_errorCode"]  = error;
                diagnostics["parsed_resultCount"] = parsed.Results.Count;
                diagnostics["parseSuccess"] = true;
            }
            else
            {
                diagnostics["parseSuccess"] = false;
                diagnostics["parseNote"] = "Results array was null after deserialization";
            }
        }
        catch (Exception ex)
        {
            diagnostics["exception"] = ex.GetType().Name;
            diagnostics["exceptionMessage"] = ex.Message;
            diagnostics["innerException"] = ex.InnerException?.Message;
        }

        return Ok(diagnostics);
    }

    private record NhtsaDebugResponse
    {
        public List<NhtsaDebugResult>? Results { get; init; }
    }

    private record NhtsaDebugResult
    {
        public string? Variable { get; init; }
        public string? Value    { get; init; }
    }
}
