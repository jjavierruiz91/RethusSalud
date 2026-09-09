using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RethusSalud.Application.Services;
using RethusSalud.Application.Validators;

namespace RethusSalud.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<DatosPersonalesValidator>();

        services.AddScoped<SolicitanteService>();
        services.AddScoped<SolicitudService>();
        services.AddScoped<CatalogoService>();
        services.AddScoped<DocumentoService>();
        services.AddScoped<ConsultaPublicaService>();
        services.AddScoped<ConfiguracionInstitucionalService>();

        return services;
    }
}
