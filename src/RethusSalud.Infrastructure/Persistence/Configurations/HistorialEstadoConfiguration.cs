using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class HistorialEstadoConfiguration : IEntityTypeConfiguration<HistorialEstado>
{
    public void Configure(EntityTypeBuilder<HistorialEstado> builder)
    {
        builder.Property(h => h.UsuarioId).IsRequired().HasMaxLength(450);
        builder.Property(h => h.Motivo).HasMaxLength(2000);
    }
}
