using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAutorFotoAComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AutorFotoUrl",
                table: "Comentarios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutorFotoUrl",
                table: "Comentarios");
        }
    }
}
