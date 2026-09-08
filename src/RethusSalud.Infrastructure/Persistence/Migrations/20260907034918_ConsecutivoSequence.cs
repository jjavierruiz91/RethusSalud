using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConsecutivoSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ConsecutivoSeq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ConsecutivoSeq");
        }
    }
}
