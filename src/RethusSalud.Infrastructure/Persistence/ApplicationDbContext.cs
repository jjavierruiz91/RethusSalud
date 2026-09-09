using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RethusSalud.Domain.Entities;
using RethusSalud.Infrastructure.Identity;

namespace RethusSalud.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Pais> Paises => Set<Pais>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Municipio> Municipios => Set<Municipio>();
    public DbSet<Profesion> Profesiones => Set<Profesion>();
    public DbSet<Solicitante> Solicitantes => Set<Solicitante>();
    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
    public DbSet<DatosAcademicos> DatosAcademicos => Set<DatosAcademicos>();
    public DbSet<ArchivoAdjunto> ArchivosAdjuntos => Set<ArchivoAdjunto>();
    public DbSet<Comentario> Comentarios => Set<Comentario>();
    public DbSet<HistorialEstado> HistorialEstados => Set<HistorialEstado>();
    public DbSet<Consecutivo> Consecutivos => Set<Consecutivo>();
    public DbSet<ConfiguracionInstitucional> ConfiguracionesInstitucionales => Set<ConfiguracionInstitucional>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        builder.HasSequence<int>("ConsecutivoSeq").StartsAt(1).IncrementsBy(1);
    }
}
