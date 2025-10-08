using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NMS_Proj.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exploradores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pontuacao = table.Column<int>(type: "INTEGER", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exploradores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sistemas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    qntdPlanetas = table.Column<int>(type: "INTEGER", nullable: false),
                    ExploradorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sistemas_Exploradores_ExploradorId",
                        column: x => x.ExploradorId,
                        principalTable: "Exploradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planetas",
                columns: table => new
                {
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    NomeQualidade = table.Column<string>(type: "TEXT", nullable: false),
                    Clima = table.Column<string>(type: "TEXT", nullable: false),
                    ClimaQualidade = table.Column<string>(type: "TEXT", nullable: false),
                    Fauna = table.Column<string>(type: "TEXT", nullable: false),
                    FaunaQualidade = table.Column<string>(type: "TEXT", nullable: false),
                    Flora = table.Column<string>(type: "TEXT", nullable: false),
                    FloraQualidade = table.Column<string>(type: "TEXT", nullable: false),
                    Sentinelas = table.Column<string>(type: "TEXT", nullable: false),
                    SentinelasQualidade = table.Column<string>(type: "TEXT", nullable: false),
                    SistemaEstelarId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExploradorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Recursos = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planetas", x => x.Nome);
                    table.ForeignKey(
                        name: "FK_Planetas_Exploradores_ExploradorId",
                        column: x => x.ExploradorId,
                        principalTable: "Exploradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Planetas_Sistemas_SistemaEstelarId",
                        column: x => x.SistemaEstelarId,
                        principalTable: "Sistemas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planetas_ExploradorId",
                table: "Planetas",
                column: "ExploradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Planetas_SistemaEstelarId",
                table: "Planetas",
                column: "SistemaEstelarId");

            migrationBuilder.CreateIndex(
                name: "IX_Sistemas_ExploradorId",
                table: "Sistemas",
                column: "ExploradorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Planetas");

            migrationBuilder.DropTable(
                name: "Sistemas");

            migrationBuilder.DropTable(
                name: "Exploradores");
        }
    }
}
