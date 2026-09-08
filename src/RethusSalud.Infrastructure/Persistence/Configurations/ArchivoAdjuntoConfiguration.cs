using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class ArchivoAdjuntoConfiguration : IEntityTypeConfiguration<ArchivoAdjunto>
{
    public void Configure(EntityTypeBuilder<ArchivoAdjunto> builder)
    {
        builder.Property(a => a.NombreArchivo).IsRequired().HasMaxLength(260);
        builder.Property(a => a.RutaAlmacenamiento).IsRequired().HasMaxLength(500);
        builder.Property(a => a.ContentType).IsRequired().HasMaxLength(150);
    }
}
