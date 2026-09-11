using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAutorYEtapaAComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutorNombre",
                table: "Comentarios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Etapa",
                table: "Comentarios",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorNombre",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "Etapa",
                table: "Comentarios");
        }
    }
}
