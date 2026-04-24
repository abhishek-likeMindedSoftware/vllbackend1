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
        services.AddDbContext<LemonLawAPIDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("LemonLawDb")));

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
