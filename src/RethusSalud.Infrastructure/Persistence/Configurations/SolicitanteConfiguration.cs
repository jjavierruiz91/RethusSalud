using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class SolicitanteConfiguration : IEntityTypeConfiguration<Solicitante>
{
    public void Configure(EntityTypeBuilder<Solicitante> builder)
    {
        builder.Property(s => s.ApplicationUserId).IsRequired().HasMaxLength(450);
        builder.Property(s => s.NumeroIdentificacion).IsRequired().HasMaxLength(30);
        builder.Property(s => s.TipoIdentificacion).HasConversion<string>().HasMaxLength(50);
        builder.Property(s => s.GrupoEtnico).HasConversion<string>().HasMaxLength(50);
        builder.Property(s => s.LugarExpedicion).HasMaxLength(150);
        builder.Property(s => s.Nombres).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Apellidos).IsRequired().HasMaxLength(150);
        builder.Property(s => s.DireccionDomicilio).HasMaxLength(250);
        builder.Property(s => s.TelefonoFijo).HasMaxLength(20);
        builder.Property(s => s.Celular).IsRequired().HasMaxLength(20);
        builder.Property(s => s.CorreoElectronico).IsRequired().HasMaxLength(200);

        builder.HasIndex(s => s.NumeroIdentificacion).IsUnique();
        builder.HasIndex(s => s.ApplicationUserId).IsUnique();

        builder.HasOne(s => s.PaisNacimiento).WithMany().HasForeignKey(s => s.PaisNacimientoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.DepartamentoNacimiento).WithMany().HasForeignKey(s => s.DepartamentoNacimientoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.MunicipioNacimiento).WithMany().HasForeignKey(s => s.MunicipioNacimientoId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.PaisResidencia).WithMany().HasForeignKey(s => s.PaisResidenciaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.DepartamentoResidencia).WithMany().HasForeignKey(s => s.DepartamentoResidenciaId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.MunicipioResidencia).WithMany().HasForeignKey(s => s.MunicipioResidenciaId).OnDelete(DeleteBehavior.Restrict);
    }
}
