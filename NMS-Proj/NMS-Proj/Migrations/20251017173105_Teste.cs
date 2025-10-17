using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NMS_Proj.Migrations
{
    /// <inheritdoc />
    public partial class Teste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeQualidade",
                table: "Planetas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeQualidade",
                table: "Planetas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
