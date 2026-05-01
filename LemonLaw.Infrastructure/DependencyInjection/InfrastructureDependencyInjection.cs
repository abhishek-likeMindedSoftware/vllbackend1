using LemonLaw.Application.Interfaces;
using LemonLaw.Application.Repositories;
using LemonLaw.Infrastructure.Data;
using LemonLaw.Infrastructure.Repositories;
using LemonLaw.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LemonLaw.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // LemonLawAPIDbContext — API's window into the shared database.
        // No MigrationsAssembly — schema is owned and managed by LemonLawDbContext (XAF).
        // Resolves connection string from "LemonLawDb" (API project) or falls back to
        // "ConnectionString" (Blazor server project) — both point to the same database.
        var connectionString =
            configuration.GetConnectionString("LemonLawDb") ??
            configuration.GetConnectionString("ConnectionString") ??
            throw new InvalidOperationException(
                "No database connection string found. Add 'LemonLawDb' or 'ConnectionString' to appsettings.json.");

        services.AddDbContext<LemonLawAPIDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IApplicationRepository, ApplicationRepository>();

        services.AddScoped<IEmailService, SendGridEmailService>();

        // Use local file storage when AzureStorage:UseLocal = true (dev without Azurite)
        var useLocal = configuration["AzureStorage:UseLocal"];
        if (string.Equals(useLocal, "true", StringComparison.OrdinalIgnoreCase))
            services.AddScoped<IDocumentStorageService, LocalFileStorageService>();
        else
            services.AddScoped<IDocumentStorageService, AzureBlobStorageService>();

        return services;
    }
}
