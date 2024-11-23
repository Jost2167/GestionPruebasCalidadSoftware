using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCalidadSoftware.Migrations
{
    /// <inheritdoc />
    public partial class AddPruebaTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PruebaTotalId",
                table: "Prueba",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PruebaTotal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PruebaTotal", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_PruebaTotalId",
                table: "Prueba",
                column: "PruebaTotalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prueba_PruebaTotal_PruebaTotalId",
                table: "Prueba",
                column: "PruebaTotalId",
                principalTable: "PruebaTotal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prueba_PruebaTotal_PruebaTotalId",
                table: "Prueba");

            migrationBuilder.DropTable(
                name: "PruebaTotal");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_PruebaTotalId",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "PruebaTotalId",
                table: "Prueba");
        }
    }
}
