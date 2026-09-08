using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Infrastructure.Persistence.Configurations;

public class DatosAcademicosConfiguration : IEntityTypeConfiguration<DatosAcademicos>
{
    public void Configure(EntityTypeBuilder<DatosAcademicos> builder)
    {
        builder.Property(d => d.TipoPrograma).IsRequired().HasMaxLength(150);
        builder.Property(d => d.NombreInstitucion).IsRequired().HasMaxLength(250);
        builder.Property(d => d.NombrePrograma).IsRequired().HasMaxLength(150);
        builder.Property(d => d.NumeroConvalidacion).HasMaxLength(50);
        builder.Property(d => d.TituloEquivalente).HasMaxLength(150);

        builder.HasOne(d => d.PaisInstitucion).WithMany().HasForeignKey(d => d.PaisInstitucionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.DepartamentoInstitucion).WithMany().HasForeignKey(d => d.DepartamentoInstitucionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.MunicipioInstitucion).WithMany().HasForeignKey(d => d.MunicipioInstitucionId).OnDelete(DeleteBehavior.Restrict);
    }
}
