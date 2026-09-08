using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class SolicitudConfiguration : IEntityTypeConfiguration<Solicitud>
{
    public void Configure(EntityTypeBuilder<Solicitud> builder)
    {
        builder.HasOne(s => s.Solicitante)
            .WithMany(s => s.Solicitudes)
            .HasForeignKey(s => s.SolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Profesion)
            .WithMany()
            .HasForeignKey(s => s.ProfesionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.DatosAcademicos)
            .WithOne(d => d.Solicitud)
            .HasForeignKey<DatosAcademicos>(d => d.SolicitudId);

        builder.HasOne(s => s.Consecutivo)
            .WithOne(c => c.Solicitud)
            .HasForeignKey<Consecutivo>(c => c.SolicitudId);

        builder.HasMany(s => s.Archivos)
            .WithOne(a => a.Solicitud)
            .HasForeignKey(a => a.SolicitudId);

        builder.HasMany(s => s.Comentarios)
            .WithOne(c => c.Solicitud)
            .HasForeignKey(c => c.SolicitudId);

        builder.HasMany(s => s.Historial)
            .WithOne(h => h.Solicitud)
            .HasForeignKey(h => h.SolicitudId);
    }
}
