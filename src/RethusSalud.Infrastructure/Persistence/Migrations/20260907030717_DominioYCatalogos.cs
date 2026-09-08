using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RethusSalud.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DominioYCatalogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TipoTramite = table.Column<int>(type: "int", nullable: false),
                    NivelFormacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departamentos_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_Departamentos_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TipoIdentificacion = table.Column<int>(type: "int", nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LugarExpedicion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PaisNacimientoId = table.Column<int>(type: "int", nullable: false),
                    DepartamentoNacimientoId = table.Column<int>(type: "int", nullable: true),
                    MunicipioNacimientoId = table.Column<int>(type: "int", nullable: true),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    PaisResidenciaId = table.Column<int>(type: "int", nullable: false),
                    DepartamentoResidenciaId = table.Column<int>(type: "int", nullable: true),
                    MunicipioResidenciaId = table.Column<int>(type: "int", nullable: true),
                    DireccionDomicilio = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TelefonoFijo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GrupoEtnico = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Departamentos_DepartamentoNacimientoId",
                        column: x => x.DepartamentoNacimientoId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Departamentos_DepartamentoResidenciaId",
                        column: x => x.DepartamentoResidenciaId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Municipios_MunicipioNacimientoId",
                        column: x => x.MunicipioNacimientoId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Municipios_MunicipioResidenciaId",
                        column: x => x.MunicipioResidenciaId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Paises_PaisNacimientoId",
                        column: x => x.PaisNacimientoId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitantes_Paises_PaisResidenciaId",
                        column: x => x.PaisResidenciaId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitanteId = table.Column<int>(type: "int", nullable: false),
                    ProfesionId = table.Column<int>(type: "int", nullable: false),
                    TipoTramite = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    EtapaActual = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Profesiones_ProfesionId",
                        column: x => x.ProfesionId,
                        principalTable: "Profesiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Solicitantes_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "Solicitantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArchivosAdjuntos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    RutaAlmacenamiento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosAdjuntos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivosAdjuntos_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    AutorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consecutivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Modo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consecutivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consecutivos_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatosAcademicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    OrigenTitulo = table.Column<int>(type: "int", nullable: false),
                    TipoInstitucion = table.Column<int>(type: "int", nullable: false),
                    TipoPrograma = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PaisInstitucionId = table.Column<int>(type: "int", nullable: false),
                    DepartamentoInstitucionId = table.Column<int>(type: "int", nullable: true),
                    MunicipioInstitucionId = table.Column<int>(type: "int", nullable: true),
                    NombreInstitucion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NombrePrograma = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FechaGrado = table.Column<DateOnly>(type: "date", nullable: false),
                    NumeroConvalidacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaConvalidacion = table.Column<DateOnly>(type: "date", nullable: true),
                    TituloEquivalente = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatosAcademicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatosAcademicos_Departamentos_DepartamentoInstitucionId",
                        column: x => x.DepartamentoInstitucionId,
                        principalTable: "Departamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatosAcademicos_Municipios_MunicipioInstitucionId",
                        column: x => x.MunicipioInstitucionId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatosAcademicos_Paises_PaisInstitucionId",
                        column: x => x.PaisInstitucionId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatosAcademicos_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolicitudId = table.Column<int>(type: "int", nullable: false),
                    EstadoResultante = table.Column<int>(type: "int", nullable: false),
                    EtapaResultante = table.Column<int>(type: "int", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstados_Solicitudes_SolicitudId",
                        column: x => x.SolicitudId,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosAdjuntos_SolicitudId",
                table: "ArchivosAdjuntos",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_SolicitudId",
                table: "Comentarios",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Consecutivos_Numero",
                table: "Consecutivos",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Consecutivos_SolicitudId",
                table: "Consecutivos",
                column: "SolicitudId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DatosAcademicos_DepartamentoInstitucionId",
                table: "DatosAcademicos",
                column: "DepartamentoInstitucionId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAcademicos_MunicipioInstitucionId",
                table: "DatosAcademicos",
                column: "MunicipioInstitucionId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAcademicos_PaisInstitucionId",
                table: "DatosAcademicos",
                column: "PaisInstitucionId");

            migrationBuilder.CreateIndex(
                name: "IX_DatosAcademicos_SolicitudId",
                table: "DatosAcademicos",
                column: "SolicitudId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departamentos_PaisId_Nombre",
                table: "Departamentos",
                columns: new[] { "PaisId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstados_SolicitudId",
                table: "HistorialEstados",
                column: "SolicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_DepartamentoId_Nombre",
                table: "Municipios",
                columns: new[] { "DepartamentoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paises_Nombre",
                table: "Paises",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profesiones_TipoTramite_Nombre",
                table: "Profesiones",
                columns: new[] { "TipoTramite", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_ApplicationUserId",
                table: "Solicitantes",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_DepartamentoNacimientoId",
                table: "Solicitantes",
                column: "DepartamentoNacimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_DepartamentoResidenciaId",
                table: "Solicitantes",
                column: "DepartamentoResidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_MunicipioNacimientoId",
                table: "Solicitantes",
                column: "MunicipioNacimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_MunicipioResidenciaId",
                table: "Solicitantes",
                column: "MunicipioResidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_NumeroIdentificacion",
                table: "Solicitantes",
                column: "NumeroIdentificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_PaisNacimientoId",
                table: "Solicitantes",
                column: "PaisNacimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitantes_PaisResidenciaId",
                table: "Solicitantes",
                column: "PaisResidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_ProfesionId",
                table: "Solicitudes",
                column: "ProfesionId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_SolicitanteId",
                table: "Solicitudes",
                column: "SolicitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivosAdjuntos");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Consecutivos");

            migrationBuilder.DropTable(
                name: "DatosAcademicos");

            migrationBuilder.DropTable(
                name: "HistorialEstados");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "Profesiones");

            migrationBuilder.DropTable(
                name: "Solicitantes");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Departamentos");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
