using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Core.Entities;
using LemonLaw.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LemonLaw.Application.Services;

/// <summary>
/// Background service that runs daily to flag overdue dealer outreach records
/// per spec §4.4. Marks outreach as OVERDUE when responseDeadline has passed
/// and no response has been received.
/// </summary>
public class OverdueOutreachBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<OverdueOutreachBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("OverdueOutreachBackgroundService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckOverdueOutreachAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in OverdueOutreachBackgroundService.");
            }

            // Run once per day
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task CheckOverdueOutreachAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var outreachRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<DealerOutreach>>();
        var eventRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<CaseEvent>>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Find all SENT outreach records past their deadline with no response
        var overdueRecords = await outreachRepo.FindAsync(o =>
            o.Status == OutreachStatus.SENT &&
            o.ResponseDeadline < today);

        int count = 0;
        foreach (var outreach in overdueRecords)
        {
            outreach.Status = OutreachStatus.OVERDUE;
            outreachRepo.Update(outreach);

            if (outreach.ApplicationId.HasValue)
            {
                await eventRepo.AddAsync(new CaseEvent
                {
                    ApplicationId = outreach.ApplicationId.Value,
                    EventType = CaseEventType.DEALER_NOTIFIED,
                    ActorType = ActorType.SYSTEM,
                    ActorDisplayName = "System",
                    Description = $"Dealer outreach marked OVERDUE. Response deadline was {outreach.ResponseDeadline}."
                });
            }
            count++;
        }

        if (count > 0)
        {
            await outreachRepo.SaveChangesAsync();
            logger.LogInformation("Marked {Count} dealer outreach records as OVERDUE.", count);
        }
    }
}
