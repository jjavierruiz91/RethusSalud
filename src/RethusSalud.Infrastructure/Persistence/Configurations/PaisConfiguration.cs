using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class PaisConfiguration : IEntityTypeConfiguration<Pais>
{
    public void Configure(EntityTypeBuilder<Pais> builder)
    {
        builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        builder.HasIndex(p => p.Nombre).IsUnique();
    }
}
