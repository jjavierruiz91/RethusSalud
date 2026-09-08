using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class ProfesionConfiguration : IEntityTypeConfiguration<Profesion>
{
    public void Configure(EntityTypeBuilder<Profesion> builder)
    {
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
        builder.HasIndex(p => new { p.TipoTramite, p.Nombre }).IsUnique();
    }
}
