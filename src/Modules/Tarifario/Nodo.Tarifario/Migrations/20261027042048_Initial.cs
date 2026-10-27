using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nodo.Tarifario.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tarifario");

            migrationBuilder.CreateTable(
                name: "Tarifarios",
                schema: "tarifario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarifas",
                schema: "tarifario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TipoRecurso = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EsHoraPico = table.Column<bool>(type: "INTEGER", nullable: false),
                    TarifaBaseMonto = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TarifaBaseMoneda = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    FranjaInicio = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    FranjaFin = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TemporadaInicio = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TemporadaFin = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TarifarioId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarifas_Tarifarios_TarifarioId",
                        column: x => x.TarifarioId,
                        principalSchema: "tarifario",
                        principalTable: "Tarifarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarifas_TarifarioId",
                schema: "tarifario",
                table: "Tarifas",
                column: "TarifarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarifas",
                schema: "tarifario");

            migrationBuilder.DropTable(
                name: "Tarifarios",
                schema: "tarifario");
        }
    }
}
