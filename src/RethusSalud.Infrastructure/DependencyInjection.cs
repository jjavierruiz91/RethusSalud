using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RethusSalud.Application.Interfaces;
using RethusSalud.Infrastructure.Identity;
using RethusSalud.Infrastructure.Persistence;
using RethusSalud.Infrastructure.Repositories;
using RethusSalud.Infrastructure.Services;
using RethusSalud.Infrastructure.Storage;
using QuestPDF.Infrastructure;

namespace RethusSalud.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<ISolicitanteRepository, SolicitanteRepository>();
        services.AddScoped<ISolicitudRepository, SolicitudRepository>();
        services.AddScoped<ICatalogoRepository, CatalogoRepository>();
        services.AddSingleton<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IConsecutivoGenerator, SqlConsecutivoGenerator>();
        services.AddScoped<ICertificadoPdfService, CertificadoPdfService>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<IReporteExcelService, ClosedXmlReporteService>();

        return services;
    }
}
