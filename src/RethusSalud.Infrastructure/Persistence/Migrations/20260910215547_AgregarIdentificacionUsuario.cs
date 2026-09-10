using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIdentificacionUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacion",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroIdentificacion",
                table: "AspNetUsers");
        }
    }
}
