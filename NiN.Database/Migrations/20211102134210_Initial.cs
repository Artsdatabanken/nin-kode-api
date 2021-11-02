using Microsoft.EntityFrameworkCore.Migrations;

namespace NiN.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "NiN_v2.3");

            migrationBuilder.CreateTable(
                name: "LKMKode",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LkmKategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKMKode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Natursystem",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natursystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtypegruppe",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatursystemId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtypegruppe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtypegruppe_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtype",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtype_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grunntype",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grunntype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grunntype_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kartleggingsenhet",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Malestokk = table.Column<int>(type: "int", nullable: false),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartleggingsenhet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kartleggingsenhet_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Miljovariabel",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miljovariabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_LKMKode_KodeId",
                        column: x => x.KodeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "LKMKode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trinn",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MiljovariabelId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trinn_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Basistrinn",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrinnId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basistrinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basistrinn_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Trinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kode",
                schema: "NiN_v2.3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodeName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasistrinnId = table.Column<int>(type: "int", nullable: true),
                    GrunntypeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: true),
                    KartleggingsenhetId = table.Column<int>(type: "int", nullable: true),
                    NatursystemId = table.Column<int>(type: "int", nullable: true),
                    TrinnId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kode_Basistrinn_BasistrinnId",
                        column: x => x.BasistrinnId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Basistrinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Kartleggingsenhet_KartleggingsenhetId",
                        column: x => x.KartleggingsenhetId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Kartleggingsenhet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalSchema: "NiN_v2.3",
                        principalTable: "Trinn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Basistrinn_TrinnId",
                schema: "NiN_v2.3",
                table: "Basistrinn",
                column: "TrinnId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_HovedtypeId",
                schema: "NiN_v2.3",
                table: "Grunntype",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_HovedtypegruppeId",
                schema: "NiN_v2.3",
                table: "Hovedtype",
                column: "HovedtypegruppeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_NatursystemId",
                schema: "NiN_v2.3",
                table: "Hovedtypegruppe",
                column: "NatursystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_HovedtypeId",
                schema: "NiN_v2.3",
                table: "Kartleggingsenhet",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_BasistrinnId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "BasistrinnId",
                unique: true,
                filter: "[BasistrinnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_GrunntypeId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "GrunntypeId",
                unique: true,
                filter: "[GrunntypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypegruppeId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "HovedtypegruppeId",
                unique: true,
                filter: "[HovedtypegruppeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypeId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "HovedtypeId",
                unique: true,
                filter: "[HovedtypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_KartleggingsenhetId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "KartleggingsenhetId",
                unique: true,
                filter: "[KartleggingsenhetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_NatursystemId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "NatursystemId",
                unique: true,
                filter: "[NatursystemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_TrinnId",
                schema: "NiN_v2.3",
                table: "Kode",
                column: "TrinnId",
                unique: true,
                filter: "[TrinnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_HovedtypeId",
                schema: "NiN_v2.3",
                table: "Miljovariabel",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_KodeId",
                schema: "NiN_v2.3",
                table: "Miljovariabel",
                column: "KodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_MiljovariabelId",
                schema: "NiN_v2.3",
                table: "Trinn",
                column: "MiljovariabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kode",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Basistrinn",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Grunntype",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Kartleggingsenhet",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Trinn",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Miljovariabel",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Hovedtype",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "LKMKode",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Hovedtypegruppe",
                schema: "NiN_v2.3");

            migrationBuilder.DropTable(
                name: "Natursystem",
                schema: "NiN_v2.3");
        }
    }
}
