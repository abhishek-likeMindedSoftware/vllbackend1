using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LemonLaw.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected string StaffId =>
        User.FindFirstValue("oid")
        ?? User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? "unknown";

    protected string StaffName =>
        User.FindFirstValue("name")
        ?? User.FindFirstValue(ClaimTypes.Name)
        ?? "Staff";

    protected string ClientIpAddress =>
        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}
