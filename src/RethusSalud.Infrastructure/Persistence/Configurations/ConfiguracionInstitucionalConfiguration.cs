using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class ConfiguracionInstitucionalConfiguration : IEntityTypeConfiguration<ConfiguracionInstitucional>
{
    public void Configure(EntityTypeBuilder<ConfiguracionInstitucional> builder)
    {
        builder.Property(c => c.Nombre).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Cargo).IsRequired().HasMaxLength(200);
    }
}
