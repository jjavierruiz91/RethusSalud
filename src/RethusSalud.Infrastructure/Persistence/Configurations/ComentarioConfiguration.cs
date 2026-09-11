using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
{
    public void Configure(EntityTypeBuilder<Comentario> builder)
    {
        builder.Property(c => c.AutorUserId).IsRequired().HasMaxLength(450);
        builder.Property(c => c.AutorNombre).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Texto).IsRequired().HasMaxLength(2000);
    }
}
