using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Application.DependencyInjection;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpClient("VinApi");

        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IVinService, VinService>();
        services.AddScoped<IVerificationService, VerificationService>();
        services.AddScoped<IDealerOutreachService, DealerOutreachService>();
        services.AddScoped<IReportingService, ReportingService>();
        services.AddScoped<IDocumentService, DocumentService>();

        // Background service for overdue dealer outreach detection (spec §4.4)
        services.AddHostedService<OverdueOutreachBackgroundService>();

        return services;
    }
}
