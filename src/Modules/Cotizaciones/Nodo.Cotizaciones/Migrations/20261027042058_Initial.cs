using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nodo.Cotizaciones.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cotizaciones");

            migrationBuilder.CreateTable(
                name: "Cotizaciones",
                schema: "cotizaciones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecursoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PrecioOfrecidoMonto = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    PrecioOfrecidoMoneda = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    VigenciaEmision = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VigenciaVencimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotizaciones", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cotizaciones",
                schema: "cotizaciones");
        }
    }
}
