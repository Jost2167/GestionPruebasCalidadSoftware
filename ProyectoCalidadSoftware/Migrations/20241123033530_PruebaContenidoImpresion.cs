using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoCalidadSoftware.Migrations
{
    /// <inheritdoc />
    public partial class PruebaContenidoImpresion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContenidoImpresion",
                table: "Prueba",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContenidoImpresion",
                table: "Prueba");
        }
    }
}
