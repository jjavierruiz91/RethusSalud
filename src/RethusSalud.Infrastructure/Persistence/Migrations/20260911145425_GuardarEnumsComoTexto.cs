using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GuardarEnumsComoTexto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoIdentificacion",
                table: "Solicitantes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(@"
                UPDATE Solicitantes SET TipoIdentificacion = CASE TipoIdentificacion
                    WHEN '1' THEN 'CedulaCiudadania'
                    WHEN '2' THEN 'CedulaExtranjeria'
                    WHEN '3' THEN 'PasaporteExtranjero'
                    WHEN '4' THEN 'PermisoProteccionTemporal'
                    ELSE TipoIdentificacion
                END");

            migrationBuilder.AlterColumn<string>(
                name: "GrupoEtnico",
                table: "Solicitantes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(@"
                UPDATE Solicitantes SET GrupoEtnico = CASE GrupoEtnico
                    WHEN '0' THEN 'NingunaDeLasAnteriores'
                    WHEN '1' THEN 'Indigena'
                    WHEN '2' THEN 'Palenquero'
                    WHEN '3' THEN 'Rom'
                    WHEN '4' THEN 'AfroDescendiente'
                    WHEN '5' THEN 'Raizal'
                    ELSE GrupoEtnico
                END");

            migrationBuilder.AlterColumn<string>(
                name: "TipoInstitucion",
                table: "DatosAcademicos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql(@"
                UPDATE DatosAcademicos SET TipoInstitucion = CASE TipoInstitucion
                    WHEN '1' THEN 'EducacionSuperior'
                    WHEN '2' THEN 'EducacionParaElTrabajo'
                    ELSE TipoInstitucion
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Solicitantes SET TipoIdentificacion = CASE TipoIdentificacion
                    WHEN 'CedulaCiudadania' THEN '1'
                    WHEN 'CedulaExtranjeria' THEN '2'
                    WHEN 'PasaporteExtranjero' THEN '3'
                    WHEN 'PermisoProteccionTemporal' THEN '4'
                    ELSE '1'
                END");

            migrationBuilder.AlterColumn<int>(
                name: "TipoIdentificacion",
                table: "Solicitantes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.Sql(@"
                UPDATE Solicitantes SET GrupoEtnico = CASE GrupoEtnico
                    WHEN 'NingunaDeLasAnteriores' THEN '0'
                    WHEN 'Indigena' THEN '1'
                    WHEN 'Palenquero' THEN '2'
                    WHEN 'Rom' THEN '3'
                    WHEN 'AfroDescendiente' THEN '4'
                    WHEN 'Raizal' THEN '5'
                    ELSE '0'
                END");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoEtnico",
                table: "Solicitantes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.Sql(@"
                UPDATE DatosAcademicos SET TipoInstitucion = CASE TipoInstitucion
                    WHEN 'EducacionSuperior' THEN '1'
                    WHEN 'EducacionParaElTrabajo' THEN '2'
                    ELSE '1'
                END");

            migrationBuilder.AlterColumn<int>(
                name: "TipoInstitucion",
                table: "DatosAcademicos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
