using LemonLaw.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonLaw.API.Controllers;

/// <summary>Returns application type metadata for the consumer portal.</summary>
[Route("api/application-types")]
[AllowAnonymous]
public class ApplicationTypesController : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(CommonResponseDto<List<ApplicationTypeDto>>), StatusCodes.Status200OK)]
    public IActionResult GetApplicationTypes()
    {
        var types = new List<ApplicationTypeDto>
        {
            new() {
                Value = "NEW_CAR",
                Label = "New Car Lemon Law",
                EligibilitySummary = "Applies to new vehicles purchased or leased within the last 36 months or 36,000 miles under the manufacturer's original warranty."
            },
            new() {
                Value = "USED_CAR",
                Label = "Used Car Warranty Law",
                EligibilitySummary = "Applies to used vehicles purchased from a dealer with an implied warranty in effect at time of sale."
            },
            new() {
                Value = "LEASED",
                Label = "Leased Vehicle Arbitration",
                EligibilitySummary = "Applies to consumers leasing a new vehicle experiencing repeated defects under the manufacturer's warranty."
            }
        };

        return Ok(new CommonResponseDto<List<ApplicationTypeDto>>
        {
            Success = true,
            Data = types
        });
    }
}
