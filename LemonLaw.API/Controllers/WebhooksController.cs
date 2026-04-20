using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LemonLaw.API.Controllers;

/// <summary>SendGrid delivery event webhook.</summary>
[Route("api/webhooks")]
[AllowAnonymous]
public class WebhooksController(
    IGenericRepository<Correspondence> correspondenceRepository) : BaseController
{
    /// <summary>Receives SendGrid delivery events and updates correspondence status.</summary>
    [HttpPost("sendgrid")]
    public async Task<IActionResult> SendGridWebhook([FromBody] JsonElement payload)
    {
        try
        {
            if (payload.ValueKind == JsonValueKind.Array)
            {
                foreach (var evt in payload.EnumerateArray())
                {
                    await ProcessSendGridEvent(evt);
                }
            }
            return Ok();
        }
        catch
        {
            return Ok(); // Always return 200 to SendGrid
        }
    }

    private async Task ProcessSendGridEvent(JsonElement evt)
    {
        if (!evt.TryGetProperty("sg_message_id", out var msgIdProp)) return;
        var messageId = msgIdProp.GetString();
        if (string.IsNullOrEmpty(messageId)) return;

        var eventType = evt.TryGetProperty("event", out var evtProp) ? evtProp.GetString() : null;

        var status = eventType switch
        {
            "delivered" => EmailDeliveryStatus.DELIVERED,
            "open" => EmailDeliveryStatus.OPENED,
            "bounce" => EmailDeliveryStatus.BOUNCED,
            "dropped" or "deferred" => EmailDeliveryStatus.FAILED,
            _ => (EmailDeliveryStatus?)null
        };

        if (status == null) return;

        var correspondence = await correspondenceRepository.FindOneAsync(
            c => c.SendGridMessageId == messageId);

        if (correspondence != null)
        {
            correspondence.DeliveryStatus = status.Value;
            correspondence.DeliveryUpdatedAt = DateTime.UtcNow;
            correspondenceRepository.Update(correspondence);
            await correspondenceRepository.SaveChangesAsync();
        }
    }
}
