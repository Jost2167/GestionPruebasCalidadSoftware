using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCalidadSoftware.Migrations
{
    /// <inheritdoc />
    public partial class RelacionPruebaPruebaTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PruebaTotalId",
                table: "Prueba",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PruebaTotalId1",
                table: "Prueba",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prueba_PruebaTotalId1",
                table: "Prueba",
                column: "PruebaTotalId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Prueba_PruebaTotal_PruebaTotalId1",
                table: "Prueba",
                column: "PruebaTotalId1",
                principalTable: "PruebaTotal",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prueba_PruebaTotal_PruebaTotalId1",
                table: "Prueba");

            migrationBuilder.DropIndex(
                name: "IX_Prueba_PruebaTotalId1",
                table: "Prueba");

            migrationBuilder.DropColumn(
                name: "PruebaTotalId1",
                table: "Prueba");

            migrationBuilder.AlterColumn<int>(
                name: "PruebaTotalId",
                table: "Prueba",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
