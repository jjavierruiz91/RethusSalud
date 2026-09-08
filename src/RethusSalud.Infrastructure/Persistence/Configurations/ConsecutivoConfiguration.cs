using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class ConsecutivoConfiguration : IEntityTypeConfiguration<Consecutivo>
{
    public void Configure(EntityTypeBuilder<Consecutivo> builder)
    {
        builder.Property(c => c.Numero).IsRequired().HasMaxLength(30);
        builder.HasIndex(c => c.Numero).IsUnique();
    }
}
