using Microsoft.EntityFrameworkCore.Migrations;

namespace NiN.Database.Migrations
{
    public partial class Grunntyper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrunntypeKartleggingsenhet",
                columns: table => new
                {
                    GrunntypeId = table.Column<int>(type: "int", nullable: false),
                    KartleggingsenhetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrunntypeKartleggingsenhet", x => new { x.GrunntypeId, x.KartleggingsenhetId });
                    table.ForeignKey(
                        name: "FK_GrunntypeKartleggingsenhet_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrunntypeKartleggingsenhet_Kartleggingsenhet_KartleggingsenhetId",
                        column: x => x.KartleggingsenhetId,
                        principalTable: "Kartleggingsenhet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrunntypeMiljovariabel",
                columns: table => new
                {
                    GrunntypeId = table.Column<int>(type: "int", nullable: false),
                    MiljovariabelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrunntypeMiljovariabel", x => new { x.GrunntypeId, x.MiljovariabelId });
                    table.ForeignKey(
                        name: "FK_GrunntypeMiljovariabel_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrunntypeMiljovariabel_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrunntypeKartleggingsenhet_KartleggingsenhetId",
                table: "GrunntypeKartleggingsenhet",
                column: "KartleggingsenhetId");

            migrationBuilder.CreateIndex(
                name: "IX_GrunntypeMiljovariabel_MiljovariabelId",
                table: "GrunntypeMiljovariabel",
                column: "MiljovariabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrunntypeKartleggingsenhet");

            migrationBuilder.DropTable(
                name: "GrunntypeMiljovariabel");
        }
    }
}
