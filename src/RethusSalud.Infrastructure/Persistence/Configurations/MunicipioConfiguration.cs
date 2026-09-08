using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.Property(m => m.Nombre).IsRequired().HasMaxLength(150);

        builder.HasOne(m => m.Departamento)
            .WithMany(d => d.Municipios)
            .HasForeignKey(m => m.DepartamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(m => new { m.DepartamentoId, m.Nombre }).IsUnique();
    }
}
