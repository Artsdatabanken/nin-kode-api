using Microsoft.EntityFrameworkCore.Migrations;

namespace NiN.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NinVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LKMKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LkmKategori = table.Column<int>(type: "int", nullable: false),
                    VersionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKMKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LKMKode_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Natursystem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natursystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Natursystem_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtypegruppe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatursystemId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtypegruppe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtypegruppe_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hovedtypegruppe_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtype_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hovedtype_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grunntype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grunntype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grunntype_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grunntype_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kartleggingsenhet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Malestokk = table.Column<int>(type: "int", nullable: false),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartleggingsenhet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kartleggingsenhet_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kartleggingsenhet_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Miljovariabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miljovariabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_LKMKode_KodeId",
                        column: x => x.KodeId,
                        principalTable: "LKMKode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trinn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MiljovariabelId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trinn_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trinn_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Basistrinn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrinnId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basistrinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basistrinn_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Basistrinn_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalTable: "Trinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasistrinnId = table.Column<int>(type: "int", nullable: true),
                    GrunntypeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: true),
                    KartleggingsenhetId = table.Column<int>(type: "int", nullable: true),
                    NatursystemId = table.Column<int>(type: "int", nullable: true),
                    TrinnId = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kode_Basistrinn_BasistrinnId",
                        column: x => x.BasistrinnId,
                        principalTable: "Basistrinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Kartleggingsenhet_KartleggingsenhetId",
                        column: x => x.KartleggingsenhetId,
                        principalTable: "Kartleggingsenhet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kode_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalTable: "Trinn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Basistrinn_TrinnId",
                table: "Basistrinn",
                column: "TrinnId");

            migrationBuilder.CreateIndex(
                name: "IX_Basistrinn_VersionId",
                table: "Basistrinn",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_HovedtypeId",
                table: "Grunntype",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_VersionId",
                table: "Grunntype",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_HovedtypegruppeId",
                table: "Hovedtype",
                column: "HovedtypegruppeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_VersionId",
                table: "Hovedtype",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_NatursystemId",
                table: "Hovedtypegruppe",
                column: "NatursystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_VersionId",
                table: "Hovedtypegruppe",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_HovedtypeId",
                table: "Kartleggingsenhet",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_VersionId",
                table: "Kartleggingsenhet",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_BasistrinnId",
                table: "Kode",
                column: "BasistrinnId",
                unique: true,
                filter: "[BasistrinnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_GrunntypeId",
                table: "Kode",
                column: "GrunntypeId",
                unique: true,
                filter: "[GrunntypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypegruppeId",
                table: "Kode",
                column: "HovedtypegruppeId",
                unique: true,
                filter: "[HovedtypegruppeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypeId",
                table: "Kode",
                column: "HovedtypeId",
                unique: true,
                filter: "[HovedtypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_KartleggingsenhetId",
                table: "Kode",
                column: "KartleggingsenhetId",
                unique: true,
                filter: "[KartleggingsenhetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_NatursystemId",
                table: "Kode",
                column: "NatursystemId",
                unique: true,
                filter: "[NatursystemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_TrinnId",
                table: "Kode",
                column: "TrinnId",
                unique: true,
                filter: "[TrinnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_VersionId",
                table: "Kode",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LKMKode_VersionId",
                table: "LKMKode",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_HovedtypeId",
                table: "Miljovariabel",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_KodeId",
                table: "Miljovariabel",
                column: "KodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_VersionId",
                table: "Miljovariabel",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Natursystem_VersionId",
                table: "Natursystem",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_MiljovariabelId",
                table: "Trinn",
                column: "MiljovariabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_VersionId",
                table: "Trinn",
                column: "VersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kode");

            migrationBuilder.DropTable(
                name: "Basistrinn");

            migrationBuilder.DropTable(
                name: "Grunntype");

            migrationBuilder.DropTable(
                name: "Kartleggingsenhet");

            migrationBuilder.DropTable(
                name: "Trinn");

            migrationBuilder.DropTable(
                name: "Miljovariabel");

            migrationBuilder.DropTable(
                name: "Hovedtype");

            migrationBuilder.DropTable(
                name: "LKMKode");

            migrationBuilder.DropTable(
                name: "Hovedtypegruppe");

            migrationBuilder.DropTable(
                name: "Natursystem");

            migrationBuilder.DropTable(
                name: "NinVersion");
        }
    }
}
