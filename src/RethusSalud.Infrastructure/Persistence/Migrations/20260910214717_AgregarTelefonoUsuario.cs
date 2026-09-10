using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTelefonoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "AspNetUsers");
        }
    }
}
