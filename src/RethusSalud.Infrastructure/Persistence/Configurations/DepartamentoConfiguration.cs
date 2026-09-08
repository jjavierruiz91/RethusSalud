using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.Property(d => d.Nombre).IsRequired().HasMaxLength(100);

        builder.HasOne(d => d.Pais)
            .WithMany(p => p.Departamentos)
            .HasForeignKey(d => d.PaisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => new { d.PaisId, d.Nombre }).IsUnique();
    }
}
